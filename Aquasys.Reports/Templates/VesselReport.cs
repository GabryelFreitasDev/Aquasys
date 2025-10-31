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

namespace Aquasys.Reports.Templates
{
    public class VesselReport : IReportTemplate
    {
        public ReportType TemplateType => ReportType.Vessel;

        public byte[] Generate(object model)
        {
            var vessel = model as Vessel
                ?? throw new ArgumentException("Modelo inválido para VesselReport");

            using var document = new PdfDocument();
            var page = document.Pages.Add();

            var fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold);
            var fontHeader = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
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

            // Helpers to read properties by several possible names (case / small variations tolerant)
            object? GetProp(object obj, params string[] candidates)
            {
                if (obj == null) return null;
                var t = obj.GetType();
                foreach (var name in candidates)
                {
                    var p = t.GetProperty(name);
                    if (p != null) return p.GetValue(obj);
                }
                // try case-insensitive search
                foreach (var p in t.GetProperties())
                {
                    if (candidates.Any(c => string.Equals(p.Name, c, StringComparison.OrdinalIgnoreCase)))
                        return p.GetValue(obj);
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

            // Title
            page.Graphics.DrawString(
                $"Relatório do Navio: {GetString(vessel, "VesselName", "Name", "vesselName")}",
                fontTitle,
                PdfBrushes.Black,
                new PointF(margin, y)
            );

            y += lineHeight + 4;

            // Vessel basic info
            EnsureSpace(lineHeight * 7);
            page.Graphics.DrawString("Informações do Navio", fontHeader, PdfBrushes.Black, new PointF(margin, y));
            y += lineHeight;

            void DrawLine(string label, string value)
            {
                page.Graphics.DrawString($"{label}: {value}", font, PdfBrushes.Black, new PointF(margin, y));
                y += lineHeight;
            }

            DrawLine("OS", GetString(vessel, "OS", "Os", "os"));
            DrawLine("Local de Docagem", GetString(vessel, "DockingLocation", "DockLocation", "DockingLocation"));
            DrawLine("IMO", GetString(vessel, "IMO", "Imo"));
            DrawLine("Porto de Registro", GetString(vessel, "PortRegistry", "PortOfRegistry", "Port"));
            DrawLine("Bandeira", GetString(vessel, "Flag", "flag")); // only name as requested
            DrawLine("Proprietário", GetString(vessel, "Owner", "Proprietary", "owner"));
            DrawLine("Operador", GetString(vessel, "VesselOperator", "Operator", "VesselOperator"));
            DrawLine("Agente de Embarque", GetString(vessel, "ShippingAgent", "Shipping_Agent", "ShippingAgent"));
            var dob = GetDate(vessel, "DateOfBuilding", "DateOfBuilt", "DateBuilt");
            if (dob.HasValue) DrawLine("Data Fabricação", dob.Value.ToString("dd/MM/yyyy"));
            var reg = GetDate(vessel, "RegistrationDateTime", "RegistrationDate", "RegisteredAt");
            if (reg.HasValue) DrawLine("Data Cadastro", reg.Value.ToString("dd/MM/yyyy"));

            // Last cargos
            EnsureSpace(lineHeight * 6);
            page.Graphics.DrawString("Últimos Cargas", fontHeader, PdfBrushes.Black, new PointF(margin, y));
            y += lineHeight;
            DrawLine("Última Carga", GetString(vessel, "LastCargo", "LastCargoDescription", "lastCargo"));
            DrawLine("2ª Última Carga", GetString(vessel, "SecondLastCargo", "SecondLastCargoDescription", "secondLastCargo"));
            DrawLine("3ª Última Carga", GetString(vessel, "ThirdLastCargo", "ThirdLastCargoDescription", "thirdLastCargo"));
            DrawLine("4ª Última Carga", GetString(vessel, "FourthLastCargo", "FourthLastCargoDescription", "fourthLastCargo"));

            // Holds and inspections
            var holdsObj = GetProp(vessel, "Holds", "HoldList", "hold", "HoldsList") as IEnumerable;
            if (holdsObj != null)
            {
                EnsureSpace(lineHeight * 2);
                page.Graphics.DrawString("Holds / Porões", fontHeader, PdfBrushes.Black, new PointF(margin, y));
                y += lineHeight;

                int holdIndex = 1;
                foreach (var hold in holdsObj)
                {
                    EnsureSpace(lineHeight * 10);
                    page.Graphics.DrawString($"Hold #{holdIndex}", fontHeader, PdfBrushes.Black, new PointF(margin + 10, y));
                    y += lineHeight;

                    DrawLine("Número do Porão", GetString(hold, "BasementNumber", "Basement", "basementNumber", "BasementNumber"));
                    DrawLine("Agente", GetString(hold, "Agent", "agent"));
                    DrawLine("Carga", GetString(hold, "Cargo", "cargo"));
                    var cap = GetDecimal(hold, "Capacity", "capacity");
                    DrawLine("Capacidade", cap.HasValue ? cap.Value.ToString("N2", CultureInfo.CurrentCulture) : string.Empty);
                    var pw = GetDecimal(hold, "ProductWeight", "productWeight", "Product_Weight");
                    DrawLine("Peso do Produto", pw.HasValue ? pw.Value.ToString("N2", CultureInfo.CurrentCulture) : string.Empty);
                    DrawLine("Plano de Carga", GetString(hold, "LoadPlan", "loadPlan"));
                    var holdReg = GetDate(hold, "RegistrationDateTime", "RegistrationDate", "RegisteredAt");
                    if (holdReg.HasValue) DrawLine("Data Registro Hold", holdReg.Value.ToString("dd/MM/yyyy"));
                    DrawLine("Inspecionado", GetString(hold, "Inspectioned", "inspectioned", "Inspectioned"));

                    // Inspection (each hold may have one inspection object)
                    var inspectionObj = GetProp(hold, "HoldInspection", "Inspection", "HoldInspections", "InspectionModel");
                    // If property is a collection, take the most recent (if enumerable)
                    if (inspectionObj is IEnumerable inspectionEnum && !(inspectionObj is string))
                    {
                        // try to pick last by RegistrationDateTime or inspectionDate
                        object? selected = null;
                        DateTime last = DateTime.MinValue;
                        foreach (var it in inspectionEnum)
                        {
                            var itDate = GetDate(it, "InspectionDate", "inspectionDate", "inspectionDateTime", "RegistrationDateTime", "registrationDateTime");
                            if (itDate.HasValue && itDate.Value > last)
                            {
                                last = itDate.Value;
                                selected = it;
                            }
                        }
                        inspectionObj = selected;
                    }

                    if (inspectionObj != null)
                    {
                        EnsureSpace(lineHeight * 8);
                        page.Graphics.DrawString("Inspeção do Hold", font, PdfBrushes.Black, new PointF(margin + 20, y));
                        y += lineHeight;

                        var inspDate = GetDate(inspectionObj, "InspectionDate", "inspectionDate", "inspectionDateTime", "inspectionDate");
                        var inspTimeObj = GetProp(inspectionObj, "InspectionTime", "inspectionTime", "Time");
                        string inspTime = inspTimeObj?.ToString() ?? string.Empty;

                        if (inspDate.HasValue) DrawLine("Data Inspeção", inspDate.Value.ToString("dd/MM/yyyy"));
                        if (!string.IsNullOrEmpty(inspTime)) DrawLine("Hora Inspeção", inspTime);
                        DrawLine("Inspetor Líder", GetString(inspectionObj, "LeadInspector", "leadInspector", "Lead_Inspector"));
                        DrawLine("Empty", GetInt(inspectionObj, "Empty", "empty")?.ToString() ?? string.Empty);
                        DrawLine("Clean", GetInt(inspectionObj, "Clean", "clean")?.ToString() ?? string.Empty);
                        DrawLine("Dry", GetInt(inspectionObj, "Dry", "dry")?.ToString() ?? string.Empty);
                        DrawLine("Odor Free", GetInt(inspectionObj, "OdorFree", "odorFree")?.ToString() ?? string.Empty);
                        DrawLine("Cargo Residue", GetInt(inspectionObj, "CargoResidue", "cargoResidue")?.ToString() ?? string.Empty);
                        DrawLine("Insects", GetInt(inspectionObj, "Insects", "insects")?.ToString() ?? string.Empty);
                        DrawLine("Método de Limpeza", GetString(inspectionObj, "CleaningMethod", "cleaningMethod"));
                        var inspReg = GetDate(inspectionObj, "RegistrationDateTime", "RegistrationDate", "registrationDateTime");
                        if (inspReg.HasValue) DrawLine("Data Registro Inspeção", inspReg.Value.ToString("dd/MM/yyyy"));
                    }

                    y += lineHeight / 2;
                    holdIndex++;
                }
            }
            else
            {
                EnsureSpace(lineHeight);
                page.Graphics.DrawString("Nenhum hold encontrado para este navio.", font, PdfBrushes.Black, new PointF(margin, y));
                y += lineHeight;
            }

            using var stream = new MemoryStream();
            document.Save(stream);
            return stream.ToArray();
        }
    }
}