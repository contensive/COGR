
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.COGR
    '
    ' Sample Vb addon
    '
    Public Class calendarDisplayClass
        Inherits AddonBaseClass
        '
        '[
        '    {
        '        "title": "My Brother Mark's Birthday",
        '        "start": "2015-12-05",
        '        "end": "2015-12-05",
        '        "starttime": "12:00:00 AM",
        '        "endtime": "12:00:00 AM",
        '        "details": "My Brother Mark's Birthday",
        '        "recordid": "102"
        '    },

        Public Class calendarEventClass
            Public title As String
            Public start As Date
            Public asdfghjklEnd As Date
            Public details As String
            Public recordid As Integer
        End Class
        '        
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Dim returnJson As String = ""
            Try
                '
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim jsonSerializer As New System.Web.Script.Serialization.JavaScriptSerializer
                Dim calendarEventList As New List(Of calendarEventClass)
                '
                returnJson = CP.Cache.Read(cacheNamecalendarEventList)
                If returnJson = "" Then
                    If cs.Open("Calendar Events") Then
                        Do
                            Dim newEvent As New calendarEventClass
                            newEvent.asdfghjklEnd = encodeMinDate(cs.GetDate("EndDate").Date())
                            newEvent.details = cs.GetText("details")
                            newEvent.recordid = cs.GetInteger("id")
                            newEvent.start = encodeMinDate(cs.GetDate("StartDate").Date())
                            newEvent.title = cs.GetText("name")
                            calendarEventList.Add(newEvent)
                            Call cs.GoNext()
                        Loop While cs.OK()
                        returnJson = jsonSerializer.Serialize(calendarEventList).Replace("asdfghjklEnd", "end")
                        'returnJson = "[" & returnJson.Substring(1) & "]"
                    End If
                    cs.Close()
                    Call CP.Cache.Save(cacheNamecalendarEventList, returnJson, "calendar events")
                End If
            Catch ex As Exception
                CP.Site.ErrorReport("exception in getEvents")
            End Try
            Return returnJson
        End Function
        '
        Private Function encodeMinDate(source As Date) As Date
            Dim returnDate As Date = source
            If returnDate < #8/7/1990# Then
                returnDate = Date.MinValue
            End If
            Return returnDate
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