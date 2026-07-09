using SVI_NFT_R.CellData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class OutShuttle
    {
		public enum EBatch
		{
			None = 0,

			MoveLoadPosition,
			MoveUnloadPosition,
		}
		public EBatch BatchCommand { get; set; } = EBatch.None;
		private string mLastSelectPosition = string.Empty;

		public bool DoManualStageMove(OutShuttleMotorX.ECommand command)
		{
			if (SetSubCommandStageMove(command) == false)
			{
				return false;
			}

			return true;
		}

		private bool SetSubCommandStageMove(OutShuttleMotorX.ECommand command)
		{
			// 인터락 체크
			if (m_objDocument.GetRunStatus() == CDefine.ERunStatus.Stop)
			{
				if (MotorStageX.m_objInterlock.CheckMotionClassInterlock(MotorStageX.MotorIndex.ToString(), (int)command) == false)
				{
					return false;
				}
			}
			// 위치 이동
			MotorStageX.SetCommand(command);
			if (MotorStageX.WaitForEndProcess() == false)
			{
				return false;
			}
			return true;
		}

		private bool SetSubCommandSelectPosition(out ECellPlacement cellPlacement)
		{
			m_objDocument.GetMainFrame().ShowWaitMessage(false);
			string selectName = string.Empty;
			cellPlacement = 0;
			try
			{
				m_objDocument.GetMainFrame().Invoke(new Action(() =>
				{
					using (var dialog = new FormEnumSelect(new string[] { "P1", "P2", "FULL" }, mLastSelectPosition))
					{
						dialog.TitleText = $"SELECT POSITION ({Resource.Get(nameof(EProcess.InShuttle))})";
						if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
						{
							return;
						}

						selectName = dialog.ResultName;
					}
				}));
			}
			finally
			{
				m_objDocument.GetMainFrame().ShowWaitMessage(true);
			}
			switch (selectName)
			{
				case "P1":
					cellPlacement |= ECellPlacement.P1;
					break;

				case "P2":
					cellPlacement |= ECellPlacement.P2;
					break;

				case "FULL":
					cellPlacement |= ECellPlacement.P1;
					cellPlacement |= ECellPlacement.P2;
					break;
			}
			return cellPlacement != 0;
		}

		private void ThreadManualProcess()
		{
			while (mbThreadExit == false)
			{
				Thread.Sleep(10);
				if (m_objDocument.GetRunStatus() != CDefine.ERunStatus.Stop)
				{
					continue;
				}

				switch (BatchCommand)
				{
					case EBatch.None:
						continue;

					case EBatch.MoveLoadPosition:
						DoManualStageMove(OutShuttleMotorX.ECommand.LoadPosition);
						break;

					case EBatch.MoveUnloadPosition:
						DoManualStageMove(OutShuttleMotorX.ECommand.UnloadPosition);
						break;

					default:
						Debug.Assert(false);
						break;
				}
				m_objDocument.SetRunStatus(CDefine.ERunStatus.Stop);
				BatchCommand = EBatch.None;
			}
		}
	}
}

