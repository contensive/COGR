
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
            Dim errorMessage As String = "ok"                '
            Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
            Dim csPeopleDomain As BaseClasses.CPCSBaseClass = CP.CSNew()
            Dim csNewMember As BaseClasses.CPCSBaseClass = CP.CSNew()
            Dim csOrgDomain As BaseClasses.CPCSBaseClass = CP.CSNew()

            Try

                Dim fullname As String = CP.Doc.GetText("fn").Trim
                Dim enteredEmail As String = CP.Doc.GetText("em").Trim '#1
                Dim deletemeexistUserInGroup As Boolean = False
                Dim actualDomain As String = String.Empty
                Dim actualUserID As Integer = 0
                Dim notificationBody As String = " "
                Dim groupId As Integer = CP.Content.GetRecordID("GROUPS", "COGR Members")
                Dim groupIdList As String = ",0," & groupId.ToString & ",0,"
                Dim cstest As CPCSBaseClass = CP.CSNew()
                Dim csorg As CPCSBaseClass = CP.CSNew()
                Dim invalidEmail As String = "Invalid Email"
                Dim InputIsEmpty As String = "empty Fullname or Email"
               

                If Not CP.User.IsAuthenticated Then
                    If CP.User.IsRecognized Then
                        CP.User.Logout()
                    End If
                End If

                CP.Utils.AppendLog("cogrLog.log", "actual email: " & enteredEmail.ToString)
                Dim emailIsUsedBySomeoneElse As Boolean = False

                If (Not String.IsNullOrEmpty(fullname)) And (Not String.IsNullOrEmpty(enteredEmail)) Then
                    ' name and email is not empty
                    If isValidEmail(CP, enteredEmail) Then
                        ' is a valid email
                        If cs.Open("people", "email=" & CP.Db.EncodeSQLText(enteredEmail)) Then
                            emailIsUsedBySomeoneElse = True
                        End If
                        Call cs.Close()
                        '
                        If emailIsUsedBySomeoneElse Then
                            '
                            ' email is already in use
                            '
                            errorMessage = "This email address is currently is use."
                        Else
                            '
                            ' email is not in use
                            '
                            'actualDomain = "@contensive.com"
                            Dim posOfEmailDomain As Integer
                            posOfEmailDomain = enteredEmail.IndexOf("@")
                            If (posOfEmailDomain < 0) Then
                                '
                                ' email does not have an @
                                '
                                errorMessage = "Your email address is not valid"
                            Else
                                '
                                ' #2.5 log them out and save the name and email
                                '
                                Dim userIsNotValid As Boolean = False
                                Call CP.User.Logout()
                                Dim csUser As CPCSBaseClass = CP.CSNew()


                                Dim FirstName As String = String.Empty ' fullname.Split(New Char() {" "c})
                                Dim LastName As String = String.Empty '= fullname.Substring(FirstName(0).Length())

                                If Not String.IsNullOrEmpty(fullname) Then
                                    If fullname.Trim.IndexOf(" ") > 0 Then
                                        FirstName = fullname.Trim.Substring(0, fullname.Trim.IndexOf(" "))
                                        LastName = fullname.Trim.Substring(fullname.Trim.IndexOf(" ") + 1)
                                    Else
                                        LastName = fullname
                                    End If

                                    ' ******************

                                    If Not csUser.Open("people", "id=" & CP.User.Id) Then
                                        userIsNotValid = True


                                    Else
                                        csUser.SetField("name", fullname)
                                        csUser.SetField("FirstName", FirstName)
                                        csUser.SetField("LastName", LastName)
                                        csUser.SetField("email", enteredEmail)

                                    End If
                                    Call csUser.Close()

                                End If


                                '
                                If userIsNotValid Then
                                    '
                                    ' user account failed
                                    '
                                    errorMessage = "Your user account could not be created"
                                Else
                                    '
                                    ' continue
                                    '
                                    actualDomain = enteredEmail.Substring(posOfEmailDomain + 1)
                                    CP.Utils.AppendLog("cogrLog.log", "actual user domain is: " & actualDomain)
                                    '
                                    '
                                    Dim orgDomain As String = ""
                                    Dim memberId As Integer = 0
                                    Dim accountId As Integer = 0
                                    Dim orgId As Integer = 0
                                    Dim sql As String
                                    sql = "select" _
                                        & " a.Name" _
                                        & " ,a.Id" _
                                        & " ,a.domain" _
                                        & " ,b.accountId" _
                                        & " ,b.orgId" _
                                        & " " _
                                        & " from organizations as a" _
                                        & " left join mmMembershipOrganizationRules as b on a.id = b.orgId" _
                                        & " " _
                                        & " where (a.domain=" & CP.Db.EncodeSQLText(actualDomain) & ")" _
                                        & " " _
                                        & " order by a.name" _
                                        & " "
                                    '
                                    Dim foundAccount As Boolean = False
                                    If Not csOrgDomain.OpenSQL(sql) Then
                                        '
                                        ' this person's emailDomain was not found in an organization with an account
                                        '
                                        editLink = CP.Content.GetCopy("COGR Member Sign Up", enteredEmail)
                                        emailBody = "" _
                                            & "<br>Full Name:" & fullname _
                                            & "<br>Email: " & enteredEmail _
                                            & "<br>" _
                                            & ""
                                        CP.Email.sendSystem("COGR Member Registration", emailBody)

                                    Else
                                        '
                                        ' add this person to the account found
                                        '
                                        foundAccount = True
                                        accountId = csOrgDomain.GetInteger("accountid")
                                        orgId = csOrgDomain.GetInteger("orgId")
                                        '
                                        If Not csPeopleDomain.Insert("membership people rules") Then
                                            '
                                            ' error inserting new people rule
                                            '
                                            foundAccount = False
                                            errorMessage = "You could not be added to this account"
                                        Else
                                            '
                                            ' create people member rule (3a)
                                            '
                                            Call csPeopleDomain.SetField("memberid", CP.User.Id)
                                            Call csPeopleDomain.SetField("accountid", accountId)
                                            Call CP.Group.AddUser("COGR Trade Members", actualUserID)
                                        End If
                                        Call csPeopleDomain.Close()
                                    End If
                                    Call csOrgDomain.Close()
                                    '
                                    '
                                    If Not CP.User.IsAuthenticated Then
                                        CP.Cache.Read("Top Menu Anonymous")
                                    Else
                                        If menu = "" Then
                                            CP.Utils.ExecuteAddon("aoMenuing.liMenuClass")

                                        End If

                                    End If

                                    '
                                    If Not foundAccount Then
                                        '
                                        ' there has been a problem so dont continue
                                        '
                                    Else
                                        '
                                        ' everything is going ok
                                        '
                                        Dim usersPassword As String
                                        Randomize()
                                        usersPassword = ((Rnd() * 899999) + 100000).ToString
                                        If cs.Open("people", "id=" & CP.User.Id) Then
                                            Call cs.SetField("username", enteredEmail)
                                            Call cs.SetField("password", usersPassword)
                                            Call cs.SetField("organizationId", orgId)
                                        End If
                                        Call cs.Close()

                                        '

                                        '
                                        emailBody = "" _
                                            & "" _
                                            & "<br>Email: " & enteredEmail _
                                            & "<br>Password: " & usersPassword _
                                            & "<br>" _
                                            & ""
                                        Call CP.Email.sendSystem("Registration Page Send New User Login Info", emailBody, CP.User.Id)
                                        '
                                        '
                                        '
                                        Call CP.User.LoginByID(CP.User.Id)
                                        '
                                        '
                                        '
                                        'Call CP.Response.Redirect("/")


                                    End If
                                    ''
                                    ''**************** Need to replace this code  *****************
                                    ''
                                    ''
                                    'If csPeopleDomain.Open("People", "email like '%" & actualDomain & "%'") Then
                                    '    '
                                    '    CP.Utils.AppendLog("cogrLog.log", "inside IF")
                                    '    Do
                                    '        actualUserID = csPeopleDomain.GetInteger("id")
                                    '        CP.Utils.AppendLog("cogrLog.log", "check user id:" & actualUserID)
                                    '        If cstest.Open("member rules", "(memberid=" & actualUserID & ")and(groupid=" & groupId & ")") Then
                                    '            '
                                    '            CP.Utils.AppendLog("cogrLog.log", "found user id:" & actualUserID)
                                    '            deletemeexistUserInGroup = True
                                    '        End If
                                    '        Call cstest.Close()

                                    '        If CP.User.IsInGroupList(groupIdList, actualUserID) Then
                                    '            '
                                    '            CP.Utils.AppendLog("cogrLog.log", "found user id:" & actualUserID)
                                    '            deletemeexistUserInGroup = True
                                    '            '
                                    '        End If
                                    '        csPeopleDomain.GoNext()
                                    '    Loop While csPeopleDomain.OK()
                                    'End If
                                    'csPeopleDomain.Close()

                                    '**************** end replace this code  *****************
                                    ' 
                                    'CP.Utils.AppendLog("cogrLog.log", "exist user in the group: " & deletemeexistUserInGroup.ToString)


                                    'If deletemeexistUserInGroup Then
                                    '    ' somebody exit in the group with the same domain

                                    '    ' create the people record
                                    '    If csNewMember.Insert("People") Then
                                    '        actualUserID = csNewMember.GetInteger("id")
                                    '        Call csNewMember.SetField("email", enteredEmail)
                                    '        Call csNewMember.SetField("name", fullname)

                                    '        ' what about username and password ?????
                                    '        ' HERE
                                    '        ' and extra logic :) - TO check
                                    '        Call csNewMember.SetField("firstname", (fullname).Substring(0, (fullname.IndexOf(" ") - 1)))
                                    '        Call csNewMember.SetField("lastname", (fullname).Substring(fullname.IndexOf(" ") + 1))
                                    '        Call csNewMember.SetField("username", enteredEmail)

                                    '        CP.Utils.AppendLog("cogrLog.log", "add the user to the group, userid : " & actualUserID)

                                    '        ' add the user to the group
                                    '        CP.Group.AddUser("COGR Members", actualUserID)

                                    '        errorMessage = "ok"
                                    '    End If
                                    '    csNewMember.Close()
                                    'Else
                                    '    ' this is the first user with that domain in the group
                                    '    ' create the user in People
                                    '    If csNewMember.Insert("People") Then
                                    '        actualUserID = csNewMember.GetInteger("id")
                                    '        Call csNewMember.SetField("email", enteredEmail)
                                    '        Call csNewMember.SetField("name", fullname)

                                    '        ' what about username and password ?????
                                    '        ' HERE
                                    '        ' and extra logic :) - TO check
                                    '        ' 
                                    '        If fullname.IndexOf(" ") > 1 Then
                                    '            Call csNewMember.SetField("firstname", (fullname).Substring(0, (fullname.IndexOf(" ") - 1)))
                                    '            Call csNewMember.SetField("lastname", (fullname).Substring(fullname.IndexOf(" ") + 1))
                                    '        Else
                                    '            Call csNewMember.SetField("firstname", fullname)
                                    '        End If
                                    '        '
                                    '        ' Send notification email to admin
                                    '        ' Here
                                    '        editLink = CP.Content.GetCopy("COGR Member Registration", enteredEmail)
                                    '        emailBody = "" _
                                    '            & "<br>Full Name:" & fullname _
                                    '            & "<br>Email: " & enteredEmail _
                                    '            & "<br>" _
                                    '            & ""
                                    '        CP.Email.sendSystem("COGR Member Registration", emailBody)

                                    '        '
                                    '        errorMessage = "ok"
                                    '    End If
                                    '    csNewMember.Close()

                                    'End If

                                End If
                                End If
                        End If
                        '

                    Else
                        ' its not a valid format
                        errorMessage = invalidEmail
                    End If
                Else
                    errorMessage = InputIsEmpty

                End If


            Catch ex As Exception
                errorReport(CP, ex, "execute")
                errorMessage = "Error"
            End Try
            Return errorMessage
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