using System.Collections.ObjectModel;
using System.Linq;

namespace SVI_NFT_R.DEVICE.Svi
{
    public static partial class ExtentionMethods
    {
        /// <summary>
        /// CIM에서 받은 메시지를 파싱해서 LinkedInspection 데이터를 만듬
        /// </summary>
        /// <param name="data"></param>
        /// <param name="linkedInspectionOrNull"></param>
        /// <returns>함수 실행 결과</returns>
        public static bool TryParseLinkedInspection(this CellLotInformationDownload data, out LinkedInspection linkedInspectionOrNull)
        {
            bool bResult = false;
            linkedInspectionOrNull = null;

            do
            {
                if (1 > data.BODY.CELLLOTS.Count)
                {
                    RaiseErrorEvent(null, string.Format("if (1 > data.BODY.CELLLOTS.Count)"));
                    break;
                }

                linkedInspectionOrNull = new LinkedInspection();
                linkedInspectionOrNull.CellID = data.BODY.CELLLOTS[0].CELLID;
                Collection<string> names = new Collection<string>();
                Collection<string> xys = new Collection<string>();
                Collection<string> imageXys = new Collection<string>();
                Collection<string> circleDraws = new Collection<string>();
                Collection<string> defectGroups = new Collection<string>();
                Collection<string> patternNames = new Collection<string>();
                string name = null, xy = null, imgXy = null, circleDraw = null, defectGroup = null, patternName = null;
                foreach (var item in data.BODY.CELLLOTS[0].ITEMS)
                {
                    switch (item.ITEM_NAME)
                    {
                        case "SEQUENCE":
                            name = xy = imgXy = circleDraw = defectGroup = patternName = null;
                            break;
                        case "NAME":
                            name = item.ITEM_VALUE;
                            break;
                        case "XY":
                            xy = item.ITEM_VALUE;
                            break;
                        case "IMG_XY":
                            imgXy = item.ITEM_VALUE;
                            break;
                        case "CIRCLE_DRAW":
                            circleDraw = item.ITEM_VALUE;
                            break;
                        case "DEFECT_GROUP":
                            defectGroup = item.ITEM_VALUE;
                            break;
                        case "PATTERN_NAME":
                            patternName = item.ITEM_VALUE;
                            break;
                    }

                    if (
                        false == string.IsNullOrWhiteSpace(name)
                        && false == string.IsNullOrWhiteSpace(xy)
                        && false == string.IsNullOrWhiteSpace(imgXy)
                        && false == string.IsNullOrWhiteSpace(circleDraw)
                        && false == string.IsNullOrWhiteSpace(defectGroup)
                        && false == string.IsNullOrWhiteSpace(patternName)
                        )
                    {
                        names.Add(name);
                        xys.Add(xy);
                        imageXys.Add(imgXy);
                        circleDraws.Add(circleDraw);
                        defectGroups.Add(defectGroup);
                        patternNames.Add(patternName);
                        name = xy = imgXy = circleDraw = defectGroup = patternName = null;
                    }
                }
                linkedInspectionOrNull.Name = names.ToArray();
                linkedInspectionOrNull.XY = xys.ToArray();
                linkedInspectionOrNull.ImageXY = imageXys.ToArray();
                linkedInspectionOrNull.CircleDraw = circleDraws.ToArray();
                linkedInspectionOrNull.DefectGroup = defectGroups.ToArray();
                linkedInspectionOrNull.PatternName = patternNames.ToArray();

                bResult = true;
            } while (false);

            return bResult;
        }
        /// <![CDATA[
        /// <?xml version="1.0" encoding="utf-8"?>
        ///<MESSAGE>
        ///    <EQPID>C1INE01_FI01</EQPID>
        ///    <NAME>CELLLOTINFODOWNLOAD</NAME>
        ///    <TRANSACTIONNO>67</TRANSACTIONNO>
        ///    <BODY>
        ///    <CELLLOTS>
        ///        <CELLLOT>
        ///        <CELLID>A4TJ1SI8DOHAH042</CELLID>
        ///        <CASSETTEID />
        ///        <BATCHLOT />
        ///        <PRODUCTID />
        ///        <PRODUCT_TYPE>DFCT</PRODUCT_TYPE>
        ///        <PRODUCT_KIND />
        ///        <PRODUCTSPEC />
        ///        <STEPID />
        ///        <PPID />
        ///        <CELL_SIZE />
        ///        <CELL_THICKNESS />
        ///        <COMMENT>PASS</COMMENT>
        ///        <ITEMS>
        ///            <ITEM>
        ///            <ITEM_NAME>SEQUENCE</ITEM_NAME>
        ///            <ITEM_VALUE>1</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>NAME</ITEM_NAME>
        ///            <ITEM_VALUE>OFF CONTIGUITY</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>XY</ITEM_NAME>
        ///            <ITEM_VALUE>1108,3</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>IMG_XY</ITEM_NAME>
        ///            <ITEM_VALUE>5695,3208</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>CIRCLE_DRAW</ITEM_NAME>
        ///            <ITEM_VALUE>1</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>DEFECT_GROUP</ITEM_NAME>
        ///            <ITEM_VALUE>0</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>PATTERN_NAME</ITEM_NAME>
        ///            <ITEM_VALUE>RED</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>SEQUENCE</ITEM_NAME>
        ///            <ITEM_VALUE>2</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>NAME</ITEM_NAME>
        ///            <ITEM_VALUE>OFF DEFECT</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>XY</ITEM_NAME>
        ///            <ITEM_VALUE>1785,2231</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>IMG_XY</ITEM_NAME>
        ///            <ITEM_VALUE>7675,1105</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>CIRCLE_DRAW</ITEM_NAME>
        ///            <ITEM_VALUE>1</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>DEFECT_GROUP</ITEM_NAME>
        ///            <ITEM_VALUE>1</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>PATTERN_NAME</ITEM_NAME>
        ///            <ITEM_VALUE>RED</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>SEQUENCE</ITEM_NAME>
        ///            <ITEM_VALUE>3</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>NAME</ITEM_NAME>
        ///            <ITEM_VALUE>HIGH CONTIGUITY</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>XY</ITEM_NAME>
        ///            <ITEM_VALUE>1108,3</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>IMG_XY</ITEM_NAME>
        ///            <ITEM_VALUE>5695,3208</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>CIRCLE_DRAW</ITEM_NAME>
        ///            <ITEM_VALUE>1</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>DEFECT_GROUP</ITEM_NAME>
        ///            <ITEM_VALUE>2</ITEM_VALUE>
        ///            </ITEM>
        ///            <ITEM>
        ///            <ITEM_NAME>PATTERN_NAME</ITEM_NAME>
        ///            <ITEM_VALUE>RED</ITEM_VALUE>
        ///            </ITEM>
        ///        </ITEMS>
        ///        </CELLLOT>
        ///    </CELLLOTS>
        ///    </BODY>
        ///</MESSAGE>
        /// ]]>

    }
}
