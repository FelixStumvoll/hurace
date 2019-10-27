select
sl.raceId,
s.id as skierId,
s.firstName,
s.lastName,
s.dateOfBirth,
s.genderId,
g.description as genderDescription,
s.countryId,
c.name,
c.countryCode,
sl.startNumber,
sl.startStateId,
ss.description as startStateDescription
from hurace.StartList as sl
join hurace.skier as s on s.id = sl.skierId
join hurace.country as c on c.id = s.countryId
join hurace.Gender as g on g.id = s.genderId
join hurace.StartState as ss on ss.id = sl.startStateId
where sl.raceId = 1
order by sl.startNumber asc