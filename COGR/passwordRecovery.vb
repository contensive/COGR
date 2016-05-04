
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR
    '
    ' Sample Vb addon
    '
    Public Class passwordRecovery

        Inherits AddonBaseClass
        '

        '        
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Try
                Dim message As String = String.Empty
                '
                Dim email As String = CP.Doc.GetText("rpw")
                Dim password As String = CP.Doc.GetText("pw")
                '
                Dim dbusername As String = String.Empty
                Dim dbpassword As String = String.Empty
                Dim loginok As Boolean = False
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim emailBody As String = "Your username and password:<br>"
                Dim passwordCnt As Integer = 0
                Dim histCount As Integer = 0
                Dim firstUserId As Integer = 0

                If CP.User.IsRecognized And Not CP.User.IsAuthenticated Then
                    CP.User.Logout()

                End If

                ' pull the information using the email
                If cs.Open("people", "email = " & CP.Db.EncodeSQLText(email)) Then
                    '
                    'passwordCnt = CP.Doc.GetInteger("numberOfEmails")

                    exitLoop = False

                    'usernameTestCnt = 0
                    Do
                        '
                        'histCount += 1
                        If firstUserId = 0 Then
                            firstUserId = cs.GetInteger("id")
                        End If
                        '
                        email = cs.GetText("email")
                        password = cs.GetText("password")
                        '

                        '
                        emailBody &= "" _
                         & "<br> E-mail: " & email _
                         & "<br> Password: " & password _
                         & "<br>" _
                         & ""
                        '
                        'If passwordCnt <> 0 Then
                        '    If histCount >= passwordCnt Then
                        '        exitLoop = True
                        '    End If
                        'End If
                        '

                        Call cs.GoNext()
                    Loop While cs.OK And (Not exitLoop)
                        '
                    CP.Email.sendSystem("Get Password", emailBody)
                    CP.Email.send(email, "webmaster@COGR.edu", "This is your userName &amp; password", emailBody, True, True)
                        'CP.Utils.AppendLog("cogrEmailLog.log", "Send Email To:" & email & "" & emailBody)
                        '
                        s = "OK"
                End If
                cs.Close()


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