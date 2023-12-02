namespace Medicines.DataProcessor
{
    using Medicines.Data;
	using Medicines.Data.Models;
	using Medicines.Data.Models.Enums;
	using Medicines.DataProcessor.ImportDtos;
	using Medicines.Utilities;
	using Newtonsoft.Json;
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;

	public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var validPatients = new HashSet<Patient>();

            var existingMedicines = context.Medicines
                .Select(m => m.Id)
                .ToHashSet();

            var importPatientDtos = JsonConvert.DeserializeObject<ImportPatientDto[]>(jsonString);

            foreach (var patientDto in importPatientDtos)
            {
                if (!IsValid(patientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patient = new Patient
                {
                    FullName = patientDto.FullName,
                    AgeGroup = (AgeGroup)patientDto.AgeGroup,
                    Gender = (Gender)patientDto.Gender
                };

                foreach (var medicineId in patientDto.Medicines)
                {
                    if (!existingMedicines.Contains(medicineId) ||
                        patient.PatientsMedicines.Any(m => m.MedicineId == medicineId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PatientMedicine patientMedicine = new PatientMedicine
                    {
                        Patient = patient,
                        MedicineId = medicineId
                    };

                    patient.PatientsMedicines.Add(patientMedicine);
                }

                validPatients.Add(patient);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));
            }

            context.Patients.AddRange(validPatients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            XmlParser xmlParser = new XmlParser();
			StringBuilder sb = new StringBuilder();
			var validPharmacies = new HashSet<Pharmacy>();

            var importPharmacyDtos = xmlParser.Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");

            foreach (var pharmacyDto in importPharmacyDtos)
            {
                if (!IsValid(pharmacyDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;  
                }

                bool isNonStop;

                try
                {
                    isNonStop = bool.Parse(pharmacyDto.IsNonStop);
                }
                catch (Exception)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy
                {
                    Name = pharmacyDto.Name,
                    PhoneNumber = pharmacyDto.PhoneNumber,
                    IsNonStop = isNonStop,
                };

                foreach (var medicineDto in pharmacyDto.Medicines)
                {
                    DateTime productionDate;
                    DateTime expiryDate;
                    string dateFormat = "yyyy-MM-dd";

                    try
                    {
                        productionDate = DateTime.ParseExact(medicineDto.ProductionDate, dateFormat, CultureInfo.InvariantCulture);
                        expiryDate = DateTime.ParseExact(medicineDto.ExpiryDate, dateFormat, CultureInfo.InvariantCulture);
					}
                    catch (Exception)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


					if (!IsValid(medicineDto) ||
                        string.IsNullOrWhiteSpace(medicineDto.Producer) ||
                        productionDate >= expiryDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medicine = new Medicine
                    {
                        Name = medicineDto.Name,
                        Price = medicineDto.Price,
                        Category = (Category)medicineDto.Category,
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = medicineDto.Producer,
                    };

                    if (pharmacy.Medicines.Any(m => m.Name == medicine.Name && m.Producer == medicine.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    pharmacy.Medicines.Add(medicine);
                }

                validPharmacies.Add(pharmacy);
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));
            }

            context.Pharmacies.AddRange(validPharmacies);
            context.SaveChanges();

			return sb.ToString().TrimEnd();
		}

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
