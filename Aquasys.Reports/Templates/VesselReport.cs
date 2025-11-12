using Aquasys.Core.Entities;
using Aquasys.Reports.Enums;
using Aquasys.Reports.Interfaces;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

namespace Aquasys.Reports.Templates
{
    public class VesselReport : IReportTemplate
    {
        public ReportType TemplateType => ReportType.Vessel;

        public byte[] Generate(object model)
        {
            var vessel = model as Vessel
                ?? throw new ArgumentException("Invalid model for VesselReport");

            using var document = new PdfDocument();
            var page = document.Pages.Add();

            var fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold);
            var fontHeader = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
            var fontSection = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            var font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

            float margin = 40;
            float y = margin;
            float lineHeight = 20;

            float pageHeight = page.GetClientSize().Height;

            void EnsureSpace(float needed)
            {
                if (y + needed > pageHeight - margin)
                {
                    page = document.Pages.Add();
                    y = margin;
                    pageHeight = page.GetClientSize().Height;
                }
            }

            object? GetProp(object obj, params string[] candidates)
            {
                if (obj == null) return null;
                var t = obj.GetType();

                foreach (var name in candidates)
                {
                    var p = t.GetProperty(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                    if (p != null) return p.GetValue(obj);
                }
                foreach (var p in t.GetProperties())
                {
                    if (candidates.Any(c => string.Equals(p.Name, c, StringComparison.OrdinalIgnoreCase)))
                        return p.GetValue(obj);
                }
                foreach (var name in candidates)
                {
                    var f = t.GetField(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                    if (f != null) return f.GetValue(obj);
                }
                foreach (var f in t.GetFields())
                {
                    if (candidates.Any(c => string.Equals(f.Name, c, StringComparison.OrdinalIgnoreCase)))
                        return f.GetValue(obj);
                }
                return null;
            }

            string GetString(object? obj, params string[] candidates)
            {
                var v = GetProp(obj ?? new { }, candidates);
                return v?.ToString() ?? string.Empty;
            }

            DateTime? GetDate(object? obj, params string[] candidates)
            {
                var v = GetProp(obj ?? new { }, candidates);
                if (v is DateTime dt) return dt;
                if (v is DateTime?) return (DateTime?)v;
                if (DateTime.TryParse(v?.ToString(), out var parsed)) return parsed;
                return null;
            }

            decimal? GetDecimal(object? obj, params string[] candidates)
            {
                var v = GetProp(obj ?? new { }, candidates);
                if (v is decimal d) return d;
                if (v is double db) return (decimal)db;
                if (v is float f) return (decimal)f;
                if (decimal.TryParse(v?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)) return parsed;
                return null;
            }

            int? GetInt(object? obj, params string[] candidates)
            {
                var v = GetProp(obj ?? new { }, candidates);
                if (v is int i) return i;
                if (int.TryParse(v?.ToString(), out var parsed)) return parsed;
                return null;
            }

            // Exibe 1/0 como True/False
            string ToBoolStr(int? val) => (val.HasValue && val.Value == 1) ? "True" : "False";

            // Title
            page.Graphics.DrawString(
                $"Vessel Report: {GetString(vessel, "VesselName", "Name", "vesselName")}",
                fontTitle,
                PdfBrushes.Black,
                new PointF(margin, y)
            );

            y += lineHeight + 4;

            // Vessel basic info
            EnsureSpace(lineHeight * 7);
            page.Graphics.DrawString("Vessel Information", fontHeader, PdfBrushes.Black, new PointF(margin, y));
            y += lineHeight;

            void DrawLine(string label, string value)
            {
                page.Graphics.DrawString($"{label}: {value}", font, PdfBrushes.Black, new PointF(margin, y));
                y += lineHeight;
            }

            DrawLine("Work Order (OS)", GetString(vessel, "OS", "Os", "os"));
            DrawLine("Docking Location", GetString(vessel, "DockingLocation", "DockLocation", "DockingLocation"));
            DrawLine("IMO", GetString(vessel, "IMO", "Imo"));
            DrawLine("Port of Registry", GetString(vessel, "PortRegistry", "PortOfRegistry", "Port"));
            DrawLine("Flag", GetString(vessel, "Flag", "flag"));
            DrawLine("Owner", GetString(vessel, "Owner", "Proprietary", "owner"));
            DrawLine("Operator", GetString(vessel, "VesselOperator", "Operator", "VesselOperator"));
            DrawLine("Shipping Agent", GetString(vessel, "ShippingAgent", "Shipping_Agent", "ShippingAgent"));
            var dob = GetDate(vessel, "DateOfBuilding", "DateOfBuilt", "DateBuilt");
            if (dob.HasValue) DrawLine("Build Date", dob.Value.ToString("MM/dd/yyyy"));
            var reg = GetDate(vessel, "RegistrationDateTime", "RegistrationDate", "RegisteredAt");
            if (reg.HasValue) DrawLine("Registration Date", reg.Value.ToString("MM/dd/yyyy"));

            // Last cargos
            EnsureSpace(lineHeight * 6);
            page.Graphics.DrawString("Last Cargos", fontHeader, PdfBrushes.Black, new PointF(margin, y));
            y += lineHeight;
            DrawLine("Last Cargo", GetString(vessel, "LastCargo", "LastCargoDescription", "lastCargo"));
            DrawLine("Second Last Cargo", GetString(vessel, "SecondLastCargo", "SecondLastCargoDescription", "secondLastCargo"));
            DrawLine("Third Last Cargo", GetString(vessel, "ThirdLastCargo", "ThirdLastCargoDescription", "thirdLastCargo"));
            DrawLine("Fourth Last Cargo", GetString(vessel, "FourthLastCargo", "FourthLastCargoDescription", "fourthLastCargo"));

            // Holds and inspections
            var holdsObj = GetProp(vessel, "Holds", "HoldList", "hold", "HoldsList") as IEnumerable;
            if (holdsObj != null)
            {
                EnsureSpace(lineHeight * 2);
                page.Graphics.DrawString("Holds", fontHeader, PdfBrushes.Black, new PointF(margin, y));
                y += lineHeight;

                int holdIndex = 1;
                foreach (var hold in holdsObj)
                {
                    EnsureSpace(lineHeight * 10);
                    // Destaque para cada porão - título maior
                    page.Graphics.DrawString($"Hold #{holdIndex}", fontHeader, PdfBrushes.Black, new PointF(margin + 10, y));
                    y += lineHeight;

                    DrawLine("Hold Number", GetString(hold, "BasementNumber", "Basement", "basementNumber", "BasementNumber"));
                    DrawLine("Agent", GetString(hold, "Agent", "agent"));
                    DrawLine("Cargo", GetString(hold, "Cargo", "cargo"));
                    var cap = GetDecimal(hold, "Capacity", "capacity");
                    DrawLine("Capacity", cap.HasValue ? cap.Value.ToString("N2", CultureInfo.InvariantCulture) : string.Empty);
                    var pw = GetDecimal(hold, "ProductWeight", "productWeight", "Product_Weight");
                    DrawLine("Product Weight", pw.HasValue ? pw.Value.ToString("N2", CultureInfo.InvariantCulture) : string.Empty);
                    DrawLine("Load Plan", GetString(hold, "LoadPlan", "loadPlan"));
                    var holdReg = GetDate(hold, "RegistrationDateTime", "RegistrationDate", "RegisteredAt");
                    if (holdReg.HasValue) DrawLine("Hold Registration Date", holdReg.Value.ToString("MM/dd/yyyy"));

                    // Inspection (garante lista também)
                    object inspectionObj = GetProp(hold, "HoldInspection", "Inspections", "HoldInspections", "InspectionModel");
                    if (inspectionObj is IList list && list.Count > 0)
                        inspectionObj = list[0];
                    else if (inspectionObj is IEnumerable enumerable && inspectionObj.GetType().IsGenericType)
                        inspectionObj = enumerable.Cast<object>().FirstOrDefault();

                    if (inspectionObj is Aquasys.Core.Entities.HoldInspection insp)
                    {
                        EnsureSpace(lineHeight * 8);
                        // TÍTULO EM DESTAQUE para inspeção
                        page.Graphics.DrawString("Hold Inspection", fontHeader, PdfBrushes.Black, new PointF(margin + 30, y));
                        y += lineHeight;

                        DrawLine("Inspection Date", insp.InspectionDateTime.ToString("MM/dd/yyyy"));
                        DrawLine("Inspection Time", insp.InspectionDateTime.ToString("HH:mm"));
                        DrawLine("Lead Inspector", insp.LeadInspector ?? string.Empty);
                        DrawLine("Empty", ToBoolStr(insp.Empty));
                        DrawLine("Clean", ToBoolStr(insp.Clean));
                        DrawLine("Dry", ToBoolStr(insp.Dry));
                        DrawLine("Odor Free", ToBoolStr(insp.OdorFree));
                        DrawLine("Cargo Residue", ToBoolStr(insp.CargoResidue));
                        DrawLine("Insects", ToBoolStr(insp.Insects));
                        DrawLine("Cleaning Method", insp.CleaningMethod ?? string.Empty);
                        DrawLine("Inspection Registration Date", insp.RegistrationDateTime.ToString("MM/dd/yyyy"));
                    }
                    else if (inspectionObj != null)
                    {
                        EnsureSpace(lineHeight * 8);
                        page.Graphics.DrawString("Hold Inspection", fontHeader, PdfBrushes.Black, new PointF(margin + 30, y));
                        y += lineHeight;

                        var inspDate = GetDate(inspectionObj, "InspectionDate", "inspectionDate", "inspectionDateTime", "inspectionDate");
                        var inspTimeObj = GetProp(inspectionObj, "InspectionTime", "inspectionTime", "Time");
                        string inspTime = inspTimeObj?.ToString() ?? string.Empty;
                        if (inspDate.HasValue) DrawLine("Inspection Date", inspDate.Value.ToString("MM/dd/yyyy"));
                        if (!string.IsNullOrEmpty(inspTime)) DrawLine("Inspection Time", inspTime);
                        DrawLine("Lead Inspector", GetString(inspectionObj, "LeadInspector", "leadInspector", "Lead_Inspector"));
                        DrawLine("Empty", ToBoolStr(GetInt(inspectionObj, "Empty", "empty")));
                        DrawLine("Clean", ToBoolStr(GetInt(inspectionObj, "Clean", "clean")));
                        DrawLine("Dry", ToBoolStr(GetInt(inspectionObj, "Dry", "dry")));
                        DrawLine("Odor Free", ToBoolStr(GetInt(inspectionObj, "OdorFree", "odorFree")));
                        DrawLine("Cargo Residue", ToBoolStr(GetInt(inspectionObj, "CargoResidue", "cargoResidue")));
                        DrawLine("Insects", ToBoolStr(GetInt(inspectionObj, "Insects", "insects")));
                        DrawLine("Cleaning Method", GetString(inspectionObj, "CleaningMethod", "cleaningMethod"));
                        var inspReg = GetDate(inspectionObj, "RegistrationDateTime", "RegistrationDate", "registrationDateTime");
                        if (inspReg.HasValue) DrawLine("Inspection Registration Date", inspReg.Value.ToString("MM/dd/yyyy"));
                    }
                    else
                    {
                        page.Graphics.DrawString("Hold Inspection", fontHeader, PdfBrushes.Black, new PointF(margin + 30, y));
                        y += lineHeight;
                        DrawLine("", "Hold not inspected");
                    }

                    y += lineHeight / 2;
                    holdIndex++;
                }
            }
            else
            {
                EnsureSpace(lineHeight);
                page.Graphics.DrawString("No holds found for this vessel.", font, PdfBrushes.Black, new PointF(margin, y));
                y += lineHeight;
            }

            using var stream = new MemoryStream();
            document.Save(stream);
            return stream.ToArray();
        }
    }
}