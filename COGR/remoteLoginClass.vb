﻿Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR
    '
    ' Sample Vb addon
    '
    Public Class remoteLoginCLass
        Inherits AddonBaseClass
        '

        '        
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Try
                Dim message As String = String.Empty
                '
                Dim email As String = CP.Doc.GetText("un")
                Dim password As String = CP.Doc.GetText("pw")
                '
                Dim dbusername As String = String.Empty
                Dim dbpassword As String = String.Empty
                Dim loginok As Boolean = False
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()


                ' pull the information using the email


                If cs.Open("People", " email = " & CP.Db.EncodeSQLText(email)) Then

                    dbusername = cs.GetText("username")
                    dbpassword = cs.GetText("password")

                    If dbpassword = password Then
                        If CP.User.Login(dbusername, password) Then
                            ' login ok
                            message = "ok"
                        Else
                            ' error login
                            message = "Error"
                        End If
                    Else
                        message = "Error"
                    End If
                Else
                    message = "Error"
                End If
                Call cs.Close()


                returnHtml = message
            Catch ex As Exception
                errorReport(CP, ex, "execute")
                returnHtml = "try error"
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