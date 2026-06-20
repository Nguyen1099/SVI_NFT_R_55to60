using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// 아날로그 모듈 켈리브레이션 데이터
        /// </summary>
        [Serializable]
        public class AnalogCalibrationParameter
        {
            /// <![CDATA[
            ///  SMC 디지털 압력 스위치 범위에 따른 공식
            ///  [Opt]           [RANGE]            [Unit]           [Slope]          [Y-Inercept]          [Comment]
            ///  in0	            0 ~ -101 	        kPa		        -25.25 	        25.25                  For vacuum
            ///  in1	            -100 ~ 100	        kPa		        50 		        -150                     For compound
            ///  in2	            0 ~ 100 	        kPa		        25 		        -25                       For low pressure
            ///  in3	            0 ~ 1 		        MPa		        0.25 	            -0.25                    For positive pressure
            ///  in4	            0 ~ 500 	        kPa		        125 	            -125                     For positive pressure
            ///  in5	            0 ~ 2 		        kPa		        0.5 	            -0.5                      For low differential pressure
            /// ]]>
            public enum EType
            {
                None = 0,
                Type1,
                Type2,
                Type3,
                Type4,
                Type5,
                Type6,
                Type7,
                Type8,
                Type9,
                Type10
            };

            /// <summary>
            /// 개별 채널 캘리브레이션 사용 여부
            /// </summary>
            public bool UsingIndividualChannelAirPressureCalibration { get; set; } = false;

            /// <summary>
            /// 채널 이름
            /// </summary>
            public string ChannelName { get; set; }

            /// <summary>
            /// 아날로그 채널 번호
            /// </summary>
            public int ChannelIndex { get; set; }

            /// <summary>
            /// 기울기 (변화량)
            /// </summary>
            public double Slope { get; set; }

            /// <summary>
            /// Y 절편
            /// </summary>
            public double YIntercept { get; set; }

            /// <summary>
            /// 타입
            /// </summary>
            public EType Type { get; set; }

            [CsvHelper.Configuration.Attributes.Ignore]
            /// <summary>
            /// 마지막으로 업데이트한 시간
            /// </summary>
            public DateTime LastUpdateDateTime { get; private set; }

            public AnalogCalibrationParameter()
            {
                LastUpdateDateTime = DateTime.Now;
            }

            /// <summary>
            /// 켈리브레이션을 적용한 값을 반환 한다.
            /// </summary>
            /// <param name="voltage">아날로그 값</param>
            /// <returns>변환된 값</returns>
            public double Convert(double voltage)
            {
                return (Slope * voltage) + YIntercept;
            }
        }

        /// <summary>
        /// 아날로그 개별 채널에대한 켈리브레이션 데이터 (공압)
        /// </summary>
        private AnalogCalibrationParameter[] mIndividualChannelAirPressureCalibrationParameters = Array.Empty<AnalogCalibrationParameter>();
        public AnalogCalibrationParameter GetIndividualChannelAnalogCalibrationParameterAirPressure(int index) => mIndividualChannelAirPressureCalibrationParameters[index];

        /// <summary>
        /// 아날로그 켈리브레이션 파라미터 반환 (공압)
        /// </summary>
        /// <param name="channelIndex">아날로그 채널 인덱스</param>  
        /// <returns>아날로그 켈리브레이션 파라미터 (해당하는 채널이 없을 경우 첫번째 채널의 켈리브레이션 파라미터 반환)</returns>
        public AnalogCalibrationParameter GetAnalogCalibrationParameterAirPressure(int channelIndex, AnalogCalibrationParameter.EType type = AnalogCalibrationParameter.EType.None)
        {
            // + 개별 캘리브레이션 데이터를 사용하는 경우 우선하여 반영한다
            if (channelIndex >= 0
                && channelIndex < mIndividualChannelAirPressureCalibrationParameters.Length
                && mIndividualChannelAirPressureCalibrationParameters[channelIndex].UsingIndividualChannelAirPressureCalibration == true
                )
            {
                return mIndividualChannelAirPressureCalibrationParameters[channelIndex];
            }

            var parameter = mIndividualChannelAirPressureCalibrationParameters.FirstOrDefault(item => item.ChannelIndex == channelIndex && item.Type == type);
            return parameter ?? mIndividualChannelAirPressureCalibrationParameters[0];
        }

        /// <summary>
        /// 아날로그 모듈 켈리브레이션 파라미터 로드 (공압)
        /// </summary>
        /// <returns>함수 실행 결과</returns>
        public bool LoadAnalogCalibrationParameterAirPressure()
        {
            bool bReturn = false;
            do
            {
                try
                {
                    mIndividualChannelAirPressureCalibrationParameters = Enum.GetValues(typeof(CDeviceIODefine.EAnalogInput))
                        .Cast<CDeviceIODefine.EAnalogInput>()
                        .Select(index =>
                        {
                            int channelIndex = (int)index;
                            var parameter = new AnalogCalibrationParameter
                            {
                                UsingIndividualChannelAirPressureCalibration = true,
                                ChannelName = index.ToString(),
                                ChannelIndex = channelIndex,
                                Slope = -25.25d,
                                YIntercept = 25.25d,
                                Type = AnalogCalibrationParameter.EType.Type1
                            };
                            return parameter;
                        })
                        .ToArray();
                    using (TextReader csvFileReader = new StreamReader(Path.Combine(GetIniPath(), CDefine.DEF_IO_INDIVIDUAL_ANALOG_CALIBRATION_TXT)))
                    {
                        CsvHelper.CsvReader csvReader = new CsvHelper.CsvReader(csvFileReader, CultureInfo.InvariantCulture);
                        csvReader.GetRecords<AnalogCalibrationParameter>().ToList().ForEach(item =>
                        {
                            if (item.ChannelIndex >= 0 && item.ChannelIndex < mIndividualChannelAirPressureCalibrationParameters.Length)
                            {
                                mIndividualChannelAirPressureCalibrationParameters[item.ChannelIndex] = item;
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    break;
                }

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 아날로그 모듈 켈리브레이션 파라미터 저장 (공압)
        /// </summary>
        /// <returns>함수 실행 결과</returns>
        public bool SaveAnalogCalibrationParameterAirPressure()
        {
            bool bReturn = false;
            do
            {
                try
                {
                    using (TextWriter csvFileWriter = new StreamWriter(Path.Combine(GetIniPath(), CDefine.DEF_IO_INDIVIDUAL_ANALOG_CALIBRATION_TXT)))
                    {
                        CsvHelper.CsvWriter csvWriter = new CsvHelper.CsvWriter(csvFileWriter, CultureInfo.InvariantCulture);
                        csvWriter.WriteRecords(mIndividualChannelAirPressureCalibrationParameters);
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    break;
                }

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 아날로그 개별 켈리브레이션 파라미터 저장 (공압)
        /// </summary>
        /// <returns>함수 실행 결과</returns>
        public bool SaveIndividualAnalogCalibrationParameterAirPressure(AnalogCalibrationParameter setValue)
        {
            bool bReturn = false;
            do
            {
                if (setValue.ChannelIndex < 0 || setValue.ChannelIndex >= mIndividualChannelAirPressureCalibrationParameters.Length)
                {
                    break;
                }
                mIndividualChannelAirPressureCalibrationParameters[setValue.ChannelIndex] = setValue.DeepClone();
                try
                {
                    using (TextWriter csvFileWriter = new StreamWriter(Path.Combine(GetIniPath(), CDefine.DEF_IO_INDIVIDUAL_ANALOG_CALIBRATION_TXT)))
                    {
                        CsvHelper.CsvWriter csvWriter = new CsvHelper.CsvWriter(csvFileWriter, CultureInfo.InvariantCulture);
                        csvWriter.WriteRecords(mIndividualChannelAirPressureCalibrationParameters);
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }
    }
}