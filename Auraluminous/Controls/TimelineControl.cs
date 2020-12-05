using System;
using MBS.Framework.Drawing;
using MBS.Framework.UserInterface.Controls.Timeline;
using MBS.Framework.UserInterface.Dialogs;
using MBS.Framework.UserInterface.Drawing;
using MBS.Framework.UserInterface.Input.Mouse;
using UniversalEditor.ObjectModels.Auraluminous.Script;
using UniversalEditor.ObjectModels.Auraluminous.Script.Commands;

namespace Auraluminous.Controls
{
	public class TimelineControl : MBS.Framework.UserInterface.Controls.Timeline.TimelineControl
	{
		protected override void RenderTimelineObject(Graphics g, MBS.Framework.UserInterface.Controls.Timeline.TimelineObject obj, Rectangle rect)
		{
			base.RenderTimelineObject(g, obj, rect);

			if (obj == null) return;

			Command command = obj.GetExtraData<Command>("command");
			if (command is ColorCommand)
			{
				ColorCommand cmd = (command as ColorCommand);
				g.FillRectangle(new LinearGradientBrush(rect, cmd.StartColor, cmd.EndColor, LinearGradientBrushOrientation.Horizontal), rect);
			}
		}

		protected override void OnObjectActivated(TimelineObjectActivatedEventArgs e)
		{
			base.OnObjectActivated(e);

			MessageDialog.ShowDialog("Object activated", "OK?", MessageDialogButtons.OK, MessageDialogIcon.Information);
		}

		protected override void OnBeforeContextMenu(EventArgs e)
		{
			base.OnBeforeContextMenu(e);

			if (e is MouseEventArgs)
			{
				TimelineItem item = HitTest(((MouseEventArgs)e).Location);
				if (item is TimelineGroup)
				{
					ContextMenuCommandID = "Auraluminous_ContextMenu_TimelineGroup";
				}
				else if (item is TimelineObject)
				{
					ContextMenuCommandID = "Auraluminous_ContextMenu_TimelineObject";
				}
			}

		}
	}
}
