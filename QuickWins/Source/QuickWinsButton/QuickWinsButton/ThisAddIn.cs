using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;

namespace QuickWinsButton
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see https://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}


Private menuBar As Office.CommandBar
Private newMenuBar As Office.CommandBarPopup
Private buttonOne As Office.CommandBarButton


Private Sub ThisApplication_Startup(ByVal sender As Object, ByVal e _
    As System.EventArgs) Handles Me.Startup
    AddMenuBar()
End Sub

Private Sub AddMenuBar()
    Try
        menuBar = Me.Application.ActiveExplorer().CommandBars.ActiveMenuBar
        newMenuBar = menuBar.Controls.Add(_
            Office.MsoControlType.msoControlPopup, _
            Temporary:= True)
        If newMenuBar IsNot Nothing Then
            newMenuBar.Caption = "New Menu"
            buttonOne = newMenuBar.Controls.Add(_
                Office.MsoControlType.msoControlButton, _
                Before:= 1, Temporary:= True)

            With buttonOne
                .Style = Office.MsoButtonStyle.msoButtonIconAndCaption
                .Caption = "Button One"
                .FaceId = 65
                .Tag = "c123"
            End With

            AddHandler buttonOne.Click, AddressOf ButtonOne_Click
            newMenuBar.Visible = True
        End If
    Catch Ex As Exception
        MsgBox(Ex.Message)
    End Try
End Sub

Public Sub ButtonOne_Click(ByVal buttonControl As Office._
CommandBarButton, ByRef Cancel As Boolean)
    MsgBox("You clicked: " & buttonControl.Caption, _
        "Custom Menu", vbOK)
End Sub
