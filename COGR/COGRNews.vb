Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR
    '
    ' Sample Vb addon
    '
    Public Class COGRNews
        Inherits AddonBaseClass
        '

        '        
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Dim csNewMember As BaseClasses.CPCSBaseClass = CP.CSNew()
            Try
                '
                Dim NewsName As String = CP.Doc.GetText("ngn")
                Dim NewsEmail As String = CP.Doc.GetText("nge")
                Dim actualUserID As Integer = 0
                Dim existUser As Boolean = False
                '            
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()

                ' pull the information using the email

                If Not CP.User.IsAuthenticated Then

                    existUser = csNewMember.Open("People", "email = " & CP.Db.EncodeSQLText(NewsEmail))
                    If Not existUser Then
                        Call csNewMember.Close()
                        csNewMember.Insert("People")
                    End If

                    actualUserID = csNewMember.GetInteger("id")
                    Call csNewMember.SetField("email", NewsEmail)
                    Call csNewMember.SetField("name", NewsName)
                    CP.Group.AddUser("Newsletter", actualUserID)
                    Call csNewMember.Close()
                Else
                    CP.Group.AddUser("Newsletter", CP.User.Id)
                End If
                'If csNewMember.Insert("People") Then
                '    actualUserID = csNewMember.GetInteger("id")
                '    Call csNewMember.SetField("email", NewsEmail)
                '    Call csNewMember.SetField("name", NewsName)
                'End If
                'CP.Group.AddUser("Newsletter", actualUserID)

                CP.Utils.AppendLog("NewsletterSignupLog.log", "Subscriber to newsletters:" & NewsName)
                'Call csNewMember.Close()

                returnHtml = "ok"
            Catch ex As Exception
                errorReport(CP, ex, "execute")
                returnHtml = "error"
            End Try
            Return returnHtml
        End Function
        '
        '=====================================================================================
        ' common report for this class
        '=====================================================================================
        '
        Private Sub errorReport(ByVal cp As CPBaseClass, ByVal ex As Exception, ByVal method As String)
            Try
                cp.Site.ErrorReport(ex, "Unexpected error in sampleClass." & method)
            Catch exLost As Exception
                '
                ' stop anything thrown from cp errorReport
                '
            End Try
        End Sub
    End Class
End Namespace