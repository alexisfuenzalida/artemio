-- ============================================================================
-- IMPORTACION: Tarifas LATAM al 13-08 (Maersk / Season 4 / Shipper 4)
-- Origen: Valparaiso / San Vicente -> 30 destinos LATAM y Caribe
-- ============================================================================
-- Como usar:
--   1) Ejecuta PASO 1 y revisa el SELECT de verificacion 1. Completa @serviceId.
--   2) Ejecuta PASO 2 y revisa el SELECT de verificacion 2.
--   3) Ejecuta PASO 3 (paises) y revisa que ningun CountryId salga NULL.
--   4) Ejecuta PASO 4 (crea los 30 puertos destino) y revisa la verificacion 4.
--   5) Recien ahi ejecuta PASO 5 (30 contratos + detalles).
--   6) Revisa el SELECT final y termina con COMMIT; (o ROLLBACK; si algo esta mal).
-- Todo corre dentro de una transaccion: nada queda guardado hasta el COMMIT.
-- ============================================================================

START TRANSACTION;

-- ============================================================================
-- PASO 1: catalogos base (Season, Shipper, Naviera, Commodity, Service, Origen)
-- ============================================================================
SET @seasonId := 4;
SET @shipperId := 4;
SET @transportCompanyId := (SELECT TransportCompanyId FROM TransportCompanies WHERE TransportCompany LIKE '%Maersk%' LIMIT 1);

INSERT INTO Commodities (Code, Commodity)
SELECT 'TARGEN', 'Tarifa General' WHERE NOT EXISTS (SELECT 1 FROM Commodities WHERE Commodity = 'Tarifa General');
SET @commodityId := (SELECT CommodityId FROM Commodities WHERE Commodity = 'Tarifa General' LIMIT 1);

SET @serviceId := NULL; -- <-- EDITAR: reemplaza NULL por el ServiceId correcto. Ver: SELECT * FROM Services;

-- Origen: intenta el nombre combinado tal como viene en la planilla; si no existe, cae a 'Valparaiso'
SET @polId := (SELECT PortId FROM Ports WHERE Port = 'Valparaiso / San Vicente' LIMIT 1);
SET @polId := COALESCE(@polId, (SELECT PortId FROM Ports WHERE Port LIKE 'Valparaiso%' LIMIT 1));

-- >>> VERIFICACION 1: ninguna columna debería salir NULL (ServiceId lo completas vos) <<<
SELECT @seasonId AS SeasonId, @shipperId AS ShipperId, @transportCompanyId AS TransportCompanyId,
       @commodityId AS CommodityId, @serviceId AS ServiceId, @polId AS POLId;

-- ============================================================================
-- PASO 2: CostItems usados en el detalle de cada contrato (se crean si faltan)
-- ============================================================================
INSERT INTO CostItems (CostItem, GroupItem)
SELECT 'FFF', 'Flete' WHERE NOT EXISTS (SELECT 1 FROM CostItems WHERE CostItem = 'FFF');
INSERT INTO CostItems (CostItem, GroupItem)
SELECT 'OHC', 'Flete' WHERE NOT EXISTS (SELECT 1 FROM CostItems WHERE CostItem = 'OHC');
INSERT INTO CostItems (CostItem, GroupItem)
SELECT 'CP+FP', 'Flete' WHERE NOT EXISTS (SELECT 1 FROM CostItems WHERE CostItem = 'CP+FP');
INSERT INTO CostItems (CostItem, GroupItem)
SELECT 'THC Destino', 'Destino' WHERE NOT EXISTS (SELECT 1 FROM CostItems WHERE CostItem = 'THC Destino');

SET @ciFff := (SELECT CostItemId FROM CostItems WHERE CostItem = 'FFF' LIMIT 1);
SET @ciOhc := (SELECT CostItemId FROM CostItems WHERE CostItem = 'OHC' LIMIT 1);
SET @ciCpFp := (SELECT CostItemId FROM CostItems WHERE CostItem = 'CP+FP' LIMIT 1);
SET @ciThc := (SELECT CostItemId FROM CostItems WHERE CostItem = 'THC Destino' LIMIT 1);

-- >>> VERIFICACION 2 <<<
SELECT @ciFff AS FFF, @ciOhc AS OHC, @ciCpFp AS CPFP, @ciThc AS THC_Destino;

-- ============================================================================
-- PASO 3: paises de los 30 destinos (se buscan en español e ingles)
-- Si alguno sale NULL, crealo en Countries antes de seguir al PASO 4.
-- ============================================================================
SELECT 'Colombia' AS Pais, (SELECT CountryId FROM Countries WHERE Country IN ('Colombia') LIMIT 1) AS CountryId
UNION ALL SELECT 'Panama', (SELECT CountryId FROM Countries WHERE Country IN ('Panama','Panamá') LIMIT 1)
UNION ALL SELECT 'Peru', (SELECT CountryId FROM Countries WHERE Country IN ('Peru','Perú') LIMIT 1)
UNION ALL SELECT 'Guatemala', (SELECT CountryId FROM Countries WHERE Country IN ('Guatemala') LIMIT 1)
UNION ALL SELECT 'Costa Rica', (SELECT CountryId FROM Countries WHERE Country IN ('Costa Rica') LIMIT 1)
UNION ALL SELECT 'El Salvador', (SELECT CountryId FROM Countries WHERE Country IN ('El Salvador') LIMIT 1)
UNION ALL SELECT 'Ecuador', (SELECT CountryId FROM Countries WHERE Country IN ('Ecuador') LIMIT 1)
UNION ALL SELECT 'Nicaragua', (SELECT CountryId FROM Countries WHERE Country IN ('Nicaragua') LIMIT 1)
UNION ALL SELECT 'Mexico', (SELECT CountryId FROM Countries WHERE Country IN ('Mexico','México') LIMIT 1)
UNION ALL SELECT 'Puerto Rico', (SELECT CountryId FROM Countries WHERE Country IN ('Puerto Rico') LIMIT 1)
UNION ALL SELECT 'Venezuela', (SELECT CountryId FROM Countries WHERE Country IN ('Venezuela') LIMIT 1)
UNION ALL SELECT 'Honduras', (SELECT CountryId FROM Countries WHERE Country IN ('Honduras') LIMIT 1)
UNION ALL SELECT 'Dominican Republic', (SELECT CountryId FROM Countries WHERE Country IN ('Dominican Republic','República Dominicana','Republica Dominicana') LIMIT 1)
UNION ALL SELECT 'Trinidad y Tobago', (SELECT CountryId FROM Countries WHERE Country IN ('Trinidad y Tobago') LIMIT 1)
UNION ALL SELECT 'Jamaica', (SELECT CountryId FROM Countries WHERE Country IN ('Jamaica') LIMIT 1)
UNION ALL SELECT 'Curacao', (SELECT CountryId FROM Countries WHERE Country IN ('Curacao','Curazao') LIMIT 1)
UNION ALL SELECT 'Aruba', (SELECT CountryId FROM Countries WHERE Country IN ('Aruba') LIMIT 1)
UNION ALL SELECT 'Haiti', (SELECT CountryId FROM Countries WHERE Country IN ('Haiti','Haití') LIMIT 1);

-- ============================================================================
-- PASO 4: crear los 30 puertos destino que falten (no toca el puerto de origen)
-- ============================================================================
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Buenaventura, Colombia', '', (SELECT CountryId FROM Countries WHERE Country IN ('Colombia') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Buenaventura, Colombia');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Manzanillo, Panama', '', (SELECT CountryId FROM Countries WHERE Country IN ('Panama','Panamá') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Manzanillo, Panama');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Callao, Peru', '', (SELECT CountryId FROM Countries WHERE Country IN ('Peru','Perú') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Callao, Peru');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Balboa, Panama', '', (SELECT CountryId FROM Countries WHERE Country IN ('Panama','Panamá') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Balboa, Panama');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Puerto Quetzal, Guatemala', '', (SELECT CountryId FROM Countries WHERE Country IN ('Guatemala') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Puerto Quetzal, Guatemala');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Puerto Caldera, Costa Rica', '', (SELECT CountryId FROM Countries WHERE Country IN ('Costa Rica') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Puerto Caldera, Costa Rica');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Acajutla, El Salvador', '', (SELECT CountryId FROM Countries WHERE Country IN ('El Salvador') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Acajutla, El Salvador');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Guayaquil, Ecuador', '', (SELECT CountryId FROM Countries WHERE Country IN ('Ecuador') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Guayaquil, Ecuador');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Posorja, Ecuador', '', (SELECT CountryId FROM Countries WHERE Country IN ('Ecuador') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Posorja, Ecuador');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Corinto, Nicaragua', '', (SELECT CountryId FROM Countries WHERE Country IN ('Nicaragua') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Corinto, Nicaragua');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Lazaro Cardenas, Mexico', '', (SELECT CountryId FROM Countries WHERE Country IN ('Mexico','México') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Lazaro Cardenas, Mexico');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Manzanillo, Mexico', '', (SELECT CountryId FROM Countries WHERE Country IN ('Mexico','México') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Manzanillo, Mexico');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Paita, Peru', '', (SELECT CountryId FROM Countries WHERE Country IN ('Peru','Perú') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Paita, Peru');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'San Juan, Puerto Rico', '', (SELECT CountryId FROM Countries WHERE Country IN ('Puerto Rico') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'San Juan, Puerto Rico');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'La Guaira, Venezuela', '', (SELECT CountryId FROM Countries WHERE Country IN ('Venezuela') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'La Guaira, Venezuela');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Cartagena, Colombia', '', (SELECT CountryId FROM Countries WHERE Country IN ('Colombia') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Cartagena, Colombia');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Puerto Cortes, Honduras', '', (SELECT CountryId FROM Countries WHERE Country IN ('Honduras') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Puerto Cortes, Honduras');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Puerto Moin, Costa Rica', '', (SELECT CountryId FROM Countries WHERE Country IN ('Costa Rica') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Puerto Moin, Costa Rica');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Caucedo, Dominican Republic', '', (SELECT CountryId FROM Countries WHERE Country IN ('Dominican Republic','República Dominicana','Republica Dominicana') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Caucedo, Dominican Republic');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Barranquilla, Colombia', '', (SELECT CountryId FROM Countries WHERE Country IN ('Colombia') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Barranquilla, Colombia');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Port-au-Prince, Haiti', '', (SELECT CountryId FROM Countries WHERE Country IN ('Haiti','Haití') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Port-au-Prince, Haiti');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Progreso, Mexico', '', (SELECT CountryId FROM Countries WHERE Country IN ('Mexico','México') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Progreso, Mexico');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Rio Haina, Dominican Republic', '', (SELECT CountryId FROM Countries WHERE Country IN ('Dominican Republic','República Dominicana','Republica Dominicana') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Rio Haina, Dominican Republic');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Puerto Cabello, Venezuela', '', (SELECT CountryId FROM Countries WHERE Country IN ('Venezuela') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Puerto Cabello, Venezuela');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Point Lisas, Trinidad y Tobago', '', (SELECT CountryId FROM Countries WHERE Country IN ('Trinidad y Tobago') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Point Lisas, Trinidad y Tobago');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Puerto Barrios, Guatemala', '', (SELECT CountryId FROM Countries WHERE Country IN ('Guatemala') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Puerto Barrios, Guatemala');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Veracruz, Mexico', '', (SELECT CountryId FROM Countries WHERE Country IN ('Mexico','México') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Veracruz, Mexico');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Kingston, Jamaica', '', (SELECT CountryId FROM Countries WHERE Country IN ('Jamaica') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Kingston, Jamaica');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Willemstad, Curacao', '', (SELECT CountryId FROM Countries WHERE Country IN ('Curacao','Curazao') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Willemstad, Curacao');
INSERT INTO Ports (Port, FreeDays, CountryId)
SELECT 'Oranjestad, Aruba', '', (SELECT CountryId FROM Countries WHERE Country IN ('Aruba') LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Ports WHERE Port = 'Oranjestad, Aruba');

-- >>> VERIFICACION 4: esta consulta NO debería devolver ninguna fila <<<
SELECT x.Port AS PuertoNoCreado FROM (
    SELECT 'Buenaventura, Colombia' AS Port UNION ALL SELECT 'Manzanillo, Panama' UNION ALL SELECT 'Callao, Peru'
    UNION ALL SELECT 'Balboa, Panama' UNION ALL SELECT 'Puerto Quetzal, Guatemala' UNION ALL SELECT 'Puerto Caldera, Costa Rica'
    UNION ALL SELECT 'Acajutla, El Salvador' UNION ALL SELECT 'Guayaquil, Ecuador' UNION ALL SELECT 'Posorja, Ecuador'
    UNION ALL SELECT 'Corinto, Nicaragua' UNION ALL SELECT 'Lazaro Cardenas, Mexico' UNION ALL SELECT 'Manzanillo, Mexico'
    UNION ALL SELECT 'Paita, Peru' UNION ALL SELECT 'San Juan, Puerto Rico' UNION ALL SELECT 'La Guaira, Venezuela'
    UNION ALL SELECT 'Cartagena, Colombia' UNION ALL SELECT 'Puerto Cortes, Honduras' UNION ALL SELECT 'Puerto Moin, Costa Rica'
    UNION ALL SELECT 'Caucedo, Dominican Republic' UNION ALL SELECT 'Barranquilla, Colombia' UNION ALL SELECT 'Port-au-Prince, Haiti'
    UNION ALL SELECT 'Progreso, Mexico' UNION ALL SELECT 'Rio Haina, Dominican Republic' UNION ALL SELECT 'Puerto Cabello, Venezuela'
    UNION ALL SELECT 'Point Lisas, Trinidad y Tobago' UNION ALL SELECT 'Puerto Barrios, Guatemala' UNION ALL SELECT 'Veracruz, Mexico'
    UNION ALL SELECT 'Kingston, Jamaica' UNION ALL SELECT 'Willemstad, Curacao' UNION ALL SELECT 'Oranjestad, Aruba'
) x
WHERE NOT EXISTS (SELECT 1 FROM Ports p WHERE p.Port = x.Port);

-- ============================================================================
-- PASO 5: los 30 contratos + sus 4 lineas de detalle (FFF, OHC, CP+FP, THC Destino)
-- Vigencia: 2026-07-01 a 2026-09-30 para todos. Status = 'Pendiente'.
-- Net = Public (BAS MSK) - VolumeDiscount (Rebate).
-- ============================================================================

-- 1) Buenaventura, Colombia -- Excel TOTAL: 2411
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-COBUN', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Buenaventura, Colombia' LIMIT 1),
        14, '', '15 DIAS COMBINADOS', 1431, 350, 1431-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 140, 'Not Include', @cid);

-- 2) Manzanillo, Panama -- Excel TOTAL: 3938
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-PAMAN', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Manzanillo, Panama' LIMIT 1),
        9, '', '10 DIAS COMBINADOS', 2958, 350, 2958-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 285, 'Not Include', @cid);

-- 3) Callao, Peru -- Excel TOTAL: 2711
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-PECAL', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Callao, Peru' LIMIT 1),
        3, '', '15 DIAS COMBINADOS', 1731, 350, 1731-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 110, 'Not Include', @cid);

-- 4) Balboa, Panama -- Excel TOTAL: 2661
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-PABLB', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Balboa, Panama' LIMIT 1),
        7, '', '10 DIAS COMBINADOS', 1681, 350, 1681-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 275, 'Not Include', @cid);

-- 5) Puerto Quetzal, Guatemala -- Excel TOTAL: 4211
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-GTTGZ', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Puerto Quetzal, Guatemala' LIMIT 1),
        19, '', '15 DIAS COMBINADOS', 3231, 350, 3231-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 280, 'Not Include', @cid);

-- 6) Puerto Caldera, Costa Rica -- Excel TOTAL: 4211
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-CRCAL', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Puerto Caldera, Costa Rica' LIMIT 1),
        11, '', '15 DIAS COMBINADOS', 3231, 350, 3231-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 265, 'Not Include', @cid);

-- 7) Acajutla, El Salvador -- Excel TOTAL: 4211
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-SVACJ', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Acajutla, El Salvador' LIMIT 1),
        25, '', '15 DIAS COMBINADOS', 3231, 350, 3231-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 255, 'Not Include', @cid);

-- 8) Guayaquil, Ecuador -- Excel TOTAL: 2011
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-ECGYE', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Guayaquil, Ecuador' LIMIT 1),
        6, '', '15 DIAS COMBINADOS / 3 PLUGGING', 1031, 350, 1031-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 225, 'Not Include', @cid);

-- 9) Posorja, Ecuador -- Excel TOTAL: 2911
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-ECPOS', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Posorja, Ecuador' LIMIT 1),
        17, '', '15 DIAS COMBINADOS', 1931, 350, 1931-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 210, 'Not Include', @cid);

-- 10) Corinto, Nicaragua -- Excel TOTAL: 4251
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-NICIO', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Corinto, Nicaragua' LIMIT 1),
        16, '', '14 DIAS COMBINADOS', 3271, 350, 3271-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 220, 'Not Include', @cid);

-- 11) Lazaro Cardenas, Mexico -- Excel TOTAL: 3411
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-MXLZC', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Lazaro Cardenas, Mexico' LIMIT 1),
        22, '', '15 DIAS COMBINADOS', 2431, 350, 2431-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 120, 'Not Include', @cid);

-- 12) Manzanillo, Mexico -- Excel TOTAL: 3411 (Rebate 700, distinto del resto)
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-MXZLO', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Manzanillo, Mexico' LIMIT 1),
        17, '', '15 DIAS COMBINADOS 3 PLUGGING', 2431, 700, 2431-700, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 120, 'Not Include', @cid);

-- 13) Paita, Peru -- Excel TOTAL: 3611
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-PEPAI', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Paita, Peru' LIMIT 1),
        21, '', '15 DIAS COMBINADOS', 2631, 350, 2631-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 110, 'Not Include', @cid);

-- 14) San Juan, Puerto Rico -- Excel TOTAL: 4638
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-PRSJU', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'San Juan, Puerto Rico' LIMIT 1),
        19, '', '15 DIAS COMBINADOS', 3658, 350, 3658-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 250, 'Not Include', @cid);

-- 15) La Guaira, Venezuela -- Excel TOTAL: 4488
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-VELAG', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'La Guaira, Venezuela' LIMIT 1),
        17, '', '15 DIAS COMBINADOS', 3508, 350, 3508-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 650, 'Not Include', @cid);

-- 16) Cartagena, Colombia -- Excel TOTAL: 4188
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-COCTG', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Cartagena, Colombia' LIMIT 1),
        14, '', '15 DIAS COMBINADOS', 3208, 350, 3208-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 140, 'Not Include', @cid);

-- 17) Puerto Cortes, Honduras -- Excel TOTAL: 4788
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-HNPCZ', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Puerto Cortes, Honduras' LIMIT 1),
        15, '', '15 DIAS COMBINADOS', 3808, 350, 3808-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 290, 'Not Include', @cid);

-- 18) Puerto Moin, Costa Rica -- Excel TOTAL: 4188
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-CRMOI', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Puerto Moin, Costa Rica' LIMIT 1),
        12, '', '15 DIAS COMBINADOS', 3208, 350, 3208-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 640, 'Not Include', @cid);

-- 19) Caucedo, Dominican Republic -- Excel TOTAL: 4638
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-DOZA6', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Caucedo, Dominican Republic' LIMIT 1),
        16, '', '10 DIAS COMBINADOS', 3658, 350, 3658-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 320, 'Not Include', @cid);

-- 20) Barranquilla, Colombia -- Excel TOTAL: 4138
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-COBAQ', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Barranquilla, Colombia' LIMIT 1),
        18, '', '15 DIAS COMBINADOS', 3158, 350, 3158-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 140, 'Not Include', @cid);

-- 21) Port-au-Prince, Haiti -- Excel TOTAL: 5538 -- SIN TT/condiciones en la planilla: TT=0, FreeDaysDestination=''
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-HTPAP', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Port-au-Prince, Haiti' LIMIT 1),
        0, '', '', 4558, 350, 4558-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 250, 'Not Include', @cid);

-- 22) Progreso, Mexico -- Excel TOTAL: 5438
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-MXPGO', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Progreso, Mexico' LIMIT 1),
        27, '', '15 DIAS COMBINADOS', 4458, 350, 4458-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 140, 'Not Include', @cid);

-- 23) Rio Haina, Dominican Republic -- Excel TOTAL: 4638
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-DOHAI', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Rio Haina, Dominican Republic' LIMIT 1),
        18, '', '10 DIAS COMBINADOS', 3658, 350, 3658-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 320, 'Not Include', @cid);

-- 24) Puerto Cabello, Venezuela -- Excel TOTAL: 4488
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-VEPBL', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Puerto Cabello, Venezuela' LIMIT 1),
        19, '', '15 DIAS COMBINADOS', 3508, 350, 3508-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 650, 'Not Include', @cid);

-- 25) Point Lisas, Trinidad y Tobago -- Excel TOTAL: 5538
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-TTPTS', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Point Lisas, Trinidad y Tobago' LIMIT 1),
        23, '', '12 DIAS COMBINADOS', 4558, 350, 4558-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 305, 'Not Include', @cid);

-- 26) Puerto Barrios, Guatemala -- Excel TOTAL: 4538
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-GTPBA', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Puerto Barrios, Guatemala' LIMIT 1),
        28, '', '10 DIAS COMBINADOS', 3558, 350, 3558-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 255, 'Not Include', @cid);

-- 27) Veracruz, Mexico -- Excel TOTAL: 4938
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-MXVER', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Veracruz, Mexico' LIMIT 1),
        14, '', '5 DIAS COMBINADOS', 3958, 350, 3958-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 140, 'Not Include', @cid);

-- 28) Kingston, Jamaica -- Excel TOTAL: 5488
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-JMKIM', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Kingston, Jamaica' LIMIT 1),
        18, '', '5 DIAS COMBINADOS', 4508, 350, 4508-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 340, 'Not Include', @cid);

-- 29) Willemstad, Curacao -- Excel TOTAL: 6788
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-CWWIL', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Willemstad, Curacao' LIMIT 1),
        18, '', '5 DIAS COMBINADOS', 5808, 350, 5808-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 530, 'Not Include', @cid);

-- 30) Oranjestad, Aruba -- Excel TOTAL: 6788 -- SIN TT/condiciones en la planilla: TT=0, FreeDaysDestination=''
INSERT INTO Contracts (TransportCompanyId, NroContract, `From`, Until, CommodityId, POLId, POT1, POT2, POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType)
VALUES (@transportCompanyId, 'LATAM-130826-AWAGU', '2026-07-01', '2026-09-30', @commodityId, @polId, '', '', '',
        (SELECT PortId FROM Ports WHERE Port = 'Oranjestad, Aruba' LIMIT 1),
        0, '', '', 5808, 350, 5808-350, 0, @serviceId,
        'SUJETO A CP3 Y RECARGOS DESTINO', @seasonId, @shipperId, 'Pendiente', 'Tarifa LATAM');
SET @cid := LAST_INSERT_ID();
INSERT INTO ContractsDetails (CostItemId, Amount, Include, ContractId) VALUES
    (@ciFff, 535, 'Not Include', @cid), (@ciOhc, 295, 'Not Include', @cid),
    (@ciCpFp, 150, 'Not Include', @cid), (@ciThc, 650, 'Not Include', @cid);

-- ============================================================================
-- VERIFICACION FINAL
-- ============================================================================
SELECT COUNT(*) AS ContratosCreados
FROM Contracts
WHERE RateType = 'Tarifa LATAM' AND NroContract LIKE 'LATAM-130826-%';

SELECT c.ContractId, c.NroContract, c.Public, c.VolumeDiscount, c.Net,
       c.Public + IFNULL((SELECT SUM(cd.Amount) FROM ContractsDetails cd WHERE cd.ContractId = c.ContractId AND cd.Include = 'Not Include'), 0) AS SumCalculado
FROM Contracts c
WHERE c.RateType = 'Tarifa LATAM' AND c.NroContract LIKE 'LATAM-130826-%'
ORDER BY c.ContractId;

-- Si los 30 contratos se ven bien (30 filas, montos correctos):
-- COMMIT;
-- Si algo esta mal:
-- ROLLBACK;
