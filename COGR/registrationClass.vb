
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR
    '
    ' Sample Vb addon
    '
    Public Class registrationClass
        Inherits AddonBaseClass
        '

        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
            Dim csPeopleDomain As BaseClasses.CPCSBaseClass = CP.CSNew()
            Dim csNewMember As BaseClasses.CPCSBaseClass = CP.CSNew()

            Try
                Dim message As String = String.Empty
                '

                Dim fullname As String = CP.Doc.GetText("fn").Trim
                Dim email As String = CP.Doc.GetText("em").Trim
                Dim existUserInGroup As Boolean = False
                Dim actualDomain As String = String.Empty
                Dim actualUserID As Integer = 0
                Dim notificationBody As String = " "
                Dim groupId As Integer = CP.Content.GetRecordID("GROUPS", "COGR Members")
                Dim groupIdList As String = ",0," & groupId.ToString & ",0,"
                Dim cstest As CPCSBaseClass = CP.CSNew()

                If Not CP.User.IsAuthenticated Then
                    If CP.User.IsRecognized Then
                        CP.User.Logout()
                    End If
                End If

                CP.Utils.AppendLog("cogrLog.log", "actual email: " & email.ToString)

                If (Not String.IsNullOrEmpty(fullname)) And (Not String.IsNullOrEmpty(email)) Then
                    ' name and email is not empty
                    If isValidEmail(CP, email) Then
                        ' is a valid email
                        If Not cs.Open("People", " email = " & CP.Db.EncodeSQLText(email)) Then
                            ' member email not exist
                            ' check the domain


                            actualDomain = "@contensive.com"
                            actualDomain = email.Substring(email.IndexOf("@"))

                            CP.Utils.AppendLog("cogrLog.log", "actual user domain is: " & actualDomain)

                            existUserInGroup = False
                            If csPeopleDomain.Open("People", "email like '%" & actualDomain & "%'") Then
                                '
                                CP.Utils.AppendLog("cogrLog.log", "inside IF")
                                Do
                                    actualUserID = csPeopleDomain.GetInteger("id")
                                    CP.Utils.AppendLog("cogrLog.log", "check user id:" & actualUserID)
                                    If cstest.Open("member rules", "(memberid=" & actualUserID & ")and(groupid=" & groupId & ")") Then
                                        '
                                        CP.Utils.AppendLog("cogrLog.log", "found user id:" & actualUserID)
                                        existUserInGroup = True
                                    End If
                                    Call cstest.Close()

                                    'If CP.User.IsInGroupList(groupIdList, actualUserID) Then
                                    '    '
                                    '    CP.Utils.AppendLog("cogrLog.log", "found user id:" & actualUserID)
                                    '    existUserInGroup = True
                                    '    '
                                    'End If
                                    csPeopleDomain.GoNext()
                                Loop While csPeopleDomain.OK()
                            End If
                            csPeopleDomain.Close()

                            ' 
                            CP.Utils.AppendLog("cogrLog.log", "exist user in the group: " & existUserInGroup.ToString)


                            If existUserInGroup Then
                                ' somebody exit in the group with the same domain

                                ' create the people record
                                If csNewMember.Insert("People") Then
                                    actualUserID = csNewMember.GetInteger("id")
                                    Call csNewMember.SetField("email", email)
                                    Call csNewMember.SetField("name", fullname)

                                    ' what about username and password ?????
                                    ' HERE
                                    ' and extra logic :) - TO check
                                    Call csNewMember.SetField("firstname", (fullname).Substring(0, (fullname.IndexOf(" ") - 1)))
                                    Call csNewMember.SetField("lastname", (fullname).Substring(fullname.IndexOf(" ") + 1))
                                    Call csNewMember.SetField("username", email)

                                    CP.Utils.AppendLog("cogrLog.log", "add the user to the group, userid : " & actualUserID)

                                    ' add the user to the group
                                    CP.Group.AddUser("COGR Members", actualUserID)

                                    message = "ok"
                                End If
                                csNewMember.Close()
                            Else
                                ' this is the first user with that domain in the group
                                ' create the user in People
                                If csNewMember.Insert("People") Then
                                    actualUserID = csNewMember.GetInteger("id")
                                    Call csNewMember.SetField("email", email)
                                    Call csNewMember.SetField("name", fullname)

                                    ' what about username and password ?????
                                    ' HERE
                                    ' and extra logic :) - TO check
                                    ' 
                                    If fullname.IndexOf(" ") > 1 Then
                                        Call csNewMember.SetField("firstname", (fullname).Substring(0, (fullname.IndexOf(" ") - 1)))
                                        Call csNewMember.SetField("lastname", (fullname).Substring(fullname.IndexOf(" ") + 1))
                                    Else
                                        Call csNewMember.SetField("firstname", fullname)
                                    End If
                                    '
                                    ' Send notification email to admin
                                    ' Here
                                    editLink = CP.Content.GetCopy("COGR Member Registration", email)
                                    emailBody = "" _
                                        & "<br>Full Name:" & fullname _
                                        & "<br>Email: " & email _
                                        & "<br>" _
                                        & ""
                                    CP.Email.sendSystem("COGR Member Registration", emailBody)
                                    
                                    '
                                    message = "ok"
                                End If
                                csNewMember.Close()

                            End If

                        Else
                            message = "error"
                        End If
                        cs.Close()
                    Else
                        ' its not a valid format
                        message = "error2"
                    End If
                Else
                    message = "error1"
                End If


                returnHtml = message
            Catch ex As Exception
                errorReport(CP, ex, "execute")
                returnHtml = ""
            End Try
            Return returnHtml
        End Function

        '
        Private Function isValidEmail(ByVal CP As CPBaseClass, email As String) As Boolean
            Dim result As Boolean = False
            Dim emailWithNotDomain As String = String.Empty
            '
            emailWithNotDomain = email.Substring(0, email.IndexOf("@"))

            CP.Utils.AppendLog("cogrLog.log", "emailWithNotDomain: " & emailWithNotDomain)

            If emailWithNotDomain.Length >= 1 Then
                result = True
            End If
            '
            Return result
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