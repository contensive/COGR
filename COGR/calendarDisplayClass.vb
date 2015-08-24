
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

        '        
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Dim returnHtml As String = ""
            Try
                '
                Dim cal As BaseClasses.CPBlockBaseClass = CP.BlockNew()
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim sS As String = ""   '   script string
                'Dim title As String
                Dim Name As String
                Dim EventStartDate As Date = Nothing
                Dim eventStart As String = ""
                Dim EventEndDate As Date = Nothing
                Dim EventDetails As String = ""
                Dim eventEnd As String = ""
                Dim EventStartTime As Date = Nothing
                Dim EventEndTime As Date = Nothing

                '
                returnHtml = CP.Cache.Read(cacheNamecalendarEventList)
                If returnHtml = "" Then
                    If cs.Open("Calendar Events") Then
                        Do
                            'title = cs.GetText("title")
                            Name = cs.GetText("name")
                            EventStartDate = encodeMinDate(cs.GetDate("StartDate"))
                            EventEndDate = encodeMinDate(cs.GetDate("EndDate"))
                            EventStartTime = encodeMinDate(cs.GetDate("StartTime"))
                            EventEndTime = encodeMinDate(cs.GetDate("EndTime"))
                            EventDetails = cs.GetText("details")
                            '
                            If (EventStartDate > Date.MinValue) And (EventEndDate > Date.MinValue) Then
                                eventStart = EventStartDate.Year() & "-" & (EventStartDate.Month() + 100).ToString.Substring(1) & "-" & (EventStartDate.Day() + 100).ToString.Substring(1)
                                eventEnd = EventEndDate.Year() & "-" & (EventEndDate.Month() + 100).ToString.Substring(1) & "-" & (EventEndDate.Day() + 100).ToString.Substring(1)

                                returnHtml &= ",{""title"": """ & CP.Utils.EncodeJavascript(Name) & """,""start"": """ & eventStart & """,""end"": """ & eventEnd & """,""starttime"": """ & EventStartTime & """,""endtime"": """ & EventEndTime & """,""details"": """ & EventDetails & """,""recordid"": """ & cs.GetInteger("id").ToString & """}" '2015-07-13
                            End If
                            Call cs.GoNext()
                        Loop While cs.OK()
                        returnHtml = "[" & returnHtml.Substring(1) & "]"

                    End If
                    cs.Close()
                    Call CP.Cache.Save(cacheNamecalendarEventList, returnHtml, "calendar events")
                End If
            Catch ex As Exception
                CP.Site.ErrorReport("exception in getEvents")
            End Try
            Return returnHtml
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