namespace Medicines.DataProcessor
{
    using Medicines.Data;
	using Medicines.DataProcessor.ExportDtos;
	using Medicines.Utilities;
	using Newtonsoft.Json;
	using System.Globalization;

	public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            XmlParser xmlParser = new XmlParser();

            string dateFormat = "yyyy-MM-dd";
            DateTime givenDate = DateTime.ParseExact(date, dateFormat, CultureInfo.InvariantCulture);


            var patients = context.Patients
                .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate > givenDate))
                .Select(p => new ExportPatientDto
                {
                    Gender = p.Gender.ToString().ToLower(),
                    Name = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Medicines = p.PatientsMedicines
                    .Where(pm => pm.Medicine.ProductionDate > givenDate)
                    .OrderByDescending(pm => pm.Medicine.ExpiryDate)
                    .ThenBy(pm => pm.Medicine.Price)
                    .Select(pm => new ExportMedicineDto
                    {
                        Category = pm.Medicine.Category.ToString().ToLower(),
                        Name = pm.Medicine.Name,
                        Price = pm.Medicine.Price.ToString("f2"),
                        Producer = pm.Medicine.Producer,
                        BestBefore = pm.Medicine.ExpiryDate.ToString(dateFormat, CultureInfo.InvariantCulture),
                    })
                    .ToList()
                })
                .OrderByDescending(p => p.Medicines.Count)
                .ThenBy(p => p.Name)
                .ToList();

            return xmlParser.Serialize(patients, "Patients");
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicines = context.Medicines
                .Where(m => (int)m.Category == medicineCategory && m.Pharmacy.IsNonStop == true)
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .Select(m => new
                {
                    m.Name,
					Price = m.Price.ToString("f2"),
					Pharmacy = new
                    {
						Name = m.Pharmacy.Name,
						PhoneNumber = m.Pharmacy.PhoneNumber
					}
                    
				})
                .ToArray();

            return JsonConvert.SerializeObject(medicines, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
