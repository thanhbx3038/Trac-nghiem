  DBCC CHECKIDENT (tests, RESEED, 0)
  select unit from questions
 
UPDATE questions
SET unit = N'Lý thuyết chuyên ngành'
WHERE unit = '3'; 
 
 update questions
 set active = 1
