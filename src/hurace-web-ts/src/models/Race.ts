import { Discipline } from './Discipline';
import { Season } from './Season';
import { Location } from './Location';
import { Gender } from './Gender';
import { RaceState } from './RaceState';

export interface Race {
    id: number;
    disciplineId: number;
    discipline: Discipline;
    seasonId: number;
    season: Season;
    locationId: number;
    location: Location;
    genderId: number;
    gender: Gender;
    raceStateId: number;
    raceState: RaceState;
    raceDate: Date;
    raceDescription?: string;
}
