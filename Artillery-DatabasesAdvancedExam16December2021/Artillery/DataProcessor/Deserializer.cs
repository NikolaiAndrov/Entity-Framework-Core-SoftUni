﻿namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Artillery.Utilities;
    using AutoMapper;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlParser xmlParser = new XmlParser();

            IMapper mapper = AutoMapperConfiguration.CreateMapper();

            ImportCountryXmlDto[] importCountriesDto = xmlParser.Deserialize<ImportCountryXmlDto[]>(xmlString, "Countries");

            ICollection<Country> validCountries = new HashSet<Country>();

            foreach (var countryDto in importCountriesDto)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = mapper.Map<Country>(countryDto);
                validCountries.Add(country);
                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(validCountries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlParser xmlParser = new XmlParser();

            IMapper mapper = AutoMapperConfiguration.CreateMapper();

            ICollection<Manufacturer> validManufacturers = new HashSet<Manufacturer>();

            ImportManufacturerXmlDto[] manufacturerDtos = xmlParser.Deserialize<ImportManufacturerXmlDto[]>(xmlString, "Manufacturers");

            ICollection<string> uniqueManufacturerNames = new HashSet<string>();

            foreach (var manufacturerDto in manufacturerDtos)
            {
                if (!IsValid(manufacturerDto) || uniqueManufacturerNames.Contains(manufacturerDto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                uniqueManufacturerNames.Add(manufacturerDto.ManufacturerName);

                string[] founded = manufacturerDto.Founded.Split(", ");
                string foundedIn = $"{founded[founded.Length - 2]}, {founded[founded.Length - 1]}";

                Manufacturer manufacturer = mapper.Map<Manufacturer>(manufacturerDto);
                validManufacturers.Add(manufacturer);
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, foundedIn));
            }

            context.Manufacturers.AddRange(validManufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlParser xmlParser = new XmlParser();

            IMapper mapper = AutoMapperConfiguration.CreateMapper();

            ImportShellXmlDto[] shellDtos = xmlParser.Deserialize<ImportShellXmlDto[]>(xmlString, "Shells");

            ICollection<Shell> validShells = new HashSet<Shell>();

            foreach (var shellDto in shellDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = mapper.Map<Shell>(shellDto);
                validShells.Add(shell);
                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportGunDto[] importGunDtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);

            ICollection<Gun> validGuns = new HashSet<Gun>();

            ICollection<int> existingManufacturers = context.Manufacturers
                .Select(x => x.Id)
                .ToHashSet();

            ICollection<int> existingShells = context.Shells
                .Select(x => x.Id)
                .ToHashSet();

            ICollection<int> existingCountries = context.Countries
                .Select(x => x.Id)
                .ToHashSet();

            foreach (var gunDto in importGunDtos)
            {
                if (!IsValid(gunDto) || 
                    !existingManufacturers.Contains(gunDto.ManufacturerId) ||
                    !existingShells.Contains(gunDto.ShellId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                if (!Enum.TryParse(gunDto.GunType, true, out GunType gunType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun
                {
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = gunType,
                    ManufacturerId = gunDto.ManufacturerId,
                    ShellId = gunDto.ShellId
                };

                foreach (var countryIdDto in gunDto.Countries)
                {
                    if (!existingCountries.Contains(countryIdDto.Id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    CountryGun countryGun = new CountryGun 
                    {
                        CountryId = countryIdDto.Id,
                        Gun = gun
                    };

                    gun.CountriesGuns.Add(countryGun);
                }

                validGuns.Add(gun);
                sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
            }

            context.Guns.AddRange(validGuns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}