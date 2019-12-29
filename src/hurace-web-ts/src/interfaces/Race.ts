import { Discipline } from './Discipline';
import { Season } from './Season';
import { Location } from './Location';

export interface Race {
    id: number;
    disciplineId: number;
    discipline: Discipline;
    seasonId: number;
    season: Season;
    locationId: number;
    location: Location;
    genderId: number;
    raceStateId: number;
    raceDate: Date;
}
