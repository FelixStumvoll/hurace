select	s.id,
		s.lastName,
		s.dateOfBirth,
		s.countryId,
		s.genderId,
		s.firstName,
		c.id as countryId,
		c.countryCode,
		c.name as countryName,
		g.id as genderId,
		g.description,
		max(td.time) as raceTime
		from hurace.TimeData as td
		join hurace.Skier as s on td.skierId = s.id
		join hurace.Country as c on s.countryId = c.id
		join hurace.Gender as g on s.genderId = g.id
		join hurace.StartList as sl on sl.raceId = td.raceId  and sl.skierId = s.id
		where td.raceId = 1 and sl.startStateId = 3
		group by
		s.id,
		s.firstName,
		s.lastName,
		s.countryId,
		s.genderId,
		s.dateOfBirth,
		c.id,
		c.countryCode,
		c.name,
		g.id,
		g.description
		order by raceTime asc