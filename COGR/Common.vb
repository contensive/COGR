Namespace Contensive.Addons.COGR
    '
    Public Module common
        '
        Public Const cacheNamecalendarEventList As String = "calendarEventList"

        Public Const loMarketList As String = ""
        Public Const loLoginForm As String = ""
        Public Const loPageList As String = ""
        Public Const loTopSlider As String = ""
        Public Const loWhatsRelated As String = ""
        Public Const loWhatsNew As String = ""

        '
        Public Const rnResourceCategoryID As String = "cogrResourceCategoryID"
        Public Const rnResourceTopicID As String = "cogrResourceTopicID"
        Public Const rnResourceTypeID As String = "cogrResourceTypeID"
        '
        Public Sub addHeadJavascript(ByVal CP As BaseClasses.CPBaseClass, ByVal scriptString As String)
            Try
                If scriptString <> "" Then
                    scriptString = "jQuery(document).ready(function(){" & scriptString & "});"
                    '
                    CP.Doc.AddHeadJavascript(scriptString)
                End If
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in Contensive.Addons.COGR.common.addHeadJavascript")
                Catch errObj As Exception
                End Try
            End Try
        End Sub
        '
        Public Sub logCOGREvent(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal eventMsg As String)
            Try
                If CP.Utils.EncodeBoolean(CP.Site.GetProperty("Allow COGR Logging")) Then
                    CP.Utils.AppendLog("cogr-feature-log.log", eventMsg)
                End If
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in Contensive.Addons.COGR.common.logCOGREvent")
                Catch errObj As Exception
                End Try
            End Try
        End Sub
        '
    End Module
    '
End Namespace


