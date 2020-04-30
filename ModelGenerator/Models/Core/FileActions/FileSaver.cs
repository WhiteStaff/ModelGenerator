
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ThreatsParser.Entities;

namespace ThreatsParser.FileActions
{
    static class FileSaver
    {
        public static void Save(List<ModelLine> model)
        {
            //TODO logic to save
        }

        private static void SaveModel(string path, List<ModelLine> model)
        {
            using (WordprocessingDocument doc =
                WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
            {
                Table table = new Table();

                // Create a TableProperties object and specify its border information.
                TableProperties tblProp = new TableProperties(
                    new TableBorders(
                        new TopBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.Apples),
                            Size = 4
                        },
                        new BottomBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.Apples),
                            Size = 4
                        },
                        new LeftBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.Apples),
                            Size = 4
                        },
                        new RightBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.Apples),
                            Size = 4
                        },
                        new InsideHorizontalBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.Apples),
                            Size = 4
                        },
                        new InsideVerticalBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.Apples),
                            Size = 4
                        }
                    )
                );
                table.AppendChild<TableProperties>(tblProp);


                table.Append(CreateRow(new ModelLine
                {
                    Id = "№ п/п",
                    Target = "Объект воздействия",
                    Source = "Источник угрозы",
                    ThreatName = "Наименование УБИ",
                    Possibility = "Вероятность",
                    Y = "Коэф. реализуемости Y",
                    Danger = "Опасность",
                    isActual = "Актуальность",
                }));

                foreach (var modelLine in model)
                {
                    table.Append(CreateRow(modelLine));

                    /*var prevCell = (TableCell)((TableRow) table.LastChild).ChildElements[1];
                    if (prevCell != null)
                    {
                        if ()
                    }*/
                }

                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                mainPart.Document = new Document(
                    new Body());

                // Append the table to the document.
                doc.MainDocumentPart.Document.Body.Append(table);
                

                doc.MainDocumentPart.Document.Save();
            }
        }

        private static TableRow CreateRow(ModelLine modelLine)
        {
            TableRow tr = new TableRow();
            TableCell tc1 = new TableCell();

            tc1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc1.Append(new Paragraph(new Run(new Text(modelLine.Id))));
            tr.Append(tc1);

            TableCell tc2 = new TableCell();
            tc2.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc2.Append(new Paragraph(new Run(new Text(modelLine.ThreatName))));
            tr.Append(tc2);

            TableCell tc3 = new TableCell();
            tc3.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc3.Append(new Paragraph(new Run(new Text(modelLine.Target))));
            tr.Append(tc3);

            TableCell tc4 = new TableCell();
            tc4.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc4.Append(new Paragraph(new Run(new Text(modelLine.Source))));
            tr.Append(tc4);

            TableCell tc5 = new TableCell();
            tc5.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc5.Append(new Paragraph(new Run(new Text(modelLine.Possibility))));
            tr.Append(tc5);

            TableCell tc6 = new TableCell();
            tc6.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc6.Append(new Paragraph(new Run(new Text(modelLine.Y))));
            tr.Append(tc6);

            TableCell tc7 = new TableCell();
            tc7.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc7.Append(new Paragraph(new Run(new Text(modelLine.Danger))));
            tr.Append(tc7);

            TableCell tc8 = new TableCell();
            tc8.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc8.Append(new Paragraph(new Run(new Text(modelLine.isActual))));
            tr.Append(tc8);

            return tr;
        }
    }
}