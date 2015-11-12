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
                '
                ' pull the information using the email
                '
                If Not String.IsNullOrEmpty(NewsEmail) Then
                    If Not CP.User.IsAuthenticated Then
                        existUser = csNewMember.Open("People", "email = " & CP.Db.EncodeSQLText(NewsEmail))
                        If Not existUser Then
                            Call csNewMember.Close()
                            csNewMember.Insert("People")
                            Call csNewMember.SetField("email", NewsEmail)
                            Call csNewMember.SetField("name", NewsName)
                        Else
                            '
                            ' this user already exists, we cannot update their record
                            '
                        End If
                        actualUserID = csNewMember.GetInteger("id")
                        CP.Group.AddUser("Newsletter", actualUserID)
                        Call csNewMember.Close()
                    Else
                        '
                        ' verify they have an email address
                        '
                        actualUserID = CP.User.Id
                        If String.IsNullOrEmpty(CP.User.Email) Then
                            If csNewMember.Open("People", "id=" & actualUserID) Then
                                csNewMember.SetField("name", NewsName)
                                csNewMember.SetField("email", NewsEmail)
                            End If
                            csNewMember.Close()
                        End If
                        CP.Group.AddUser("Newsletter", actualUserID)
                    End If

                    emailBody = "" _
                                                 & "<br>Full Name:" & NewsName _
                                                 & "<br>Email: " & NewsEmail _
                                                 & "<br>" _
                                                 & ""
                    CP.Email.sendSystem("COGR Listserv Registration", emailBody)
                    '
                    CP.Email.sendSystem("COGR Listserv Auto Responder", , actualUserID)

                    CP.Utils.AppendLog("NewsletterSignupLog.log", "Subscriber to newsletters:" & emailBody)
                    'Call csNewMember.Close()

                End If
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