
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR
    '
    ' Sample Vb addon
    '
    Public Class remoteMethodCalEdit
        Inherits AddonBaseClass
        '

        '        
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Try
                '
                Dim userCanEdit As Boolean = False
                '

                If CP.User.IsAuthenticated Then
                    If CP.User.IsInGroup("AllowCalendarEdit") Then
                        ' AllowCalendarEdit
                        userCanEdit = True
                    End If
                End If

                '
                If userCanEdit Then
                    returnHtml = "ok"
                Else
                    returnHtml = "error"
                End If
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
