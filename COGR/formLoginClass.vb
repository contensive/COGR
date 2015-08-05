Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR
    '
    ' Sample Vb addon
    '
    Public Class formloginCLass
        Inherits AddonBaseClass
        '

        '        
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Try
                '
                Dim email As String = CP.Doc.GetText("un")
                Dim password As String = CP.Doc.GetText("pw")
                '
                Dim dbusername As String = String.Empty
                Dim dbpassword As String = String.Empty
                Dim loginok As Boolean = False
                Dim s As String = String.Empty
                Dim layout As BaseClasses.CPBlockBaseClass = CP.BlockNew()
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()


                If Not CP.User.IsAuthenticated Then
                    If CP.User.IsRecognized Then
                        CP.User.Logout()
                    End If

                    Call layout.openLayout("COGR Login Form Layout")
                    s = layout.getHtml()
                Else

                    Call layout.OpenLayout("COGR Logout Form Layout")
                    s = layout.GetHtml()

                End If

                returnHtml = s
            Catch ex As Exception
                errorReport(CP, ex, "execute")
                returnHtml = "html error"
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