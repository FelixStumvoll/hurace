import { Skier } from '../models/Skier';
import { Race } from '../models/Race';
import { Season } from '../models/Season';
import { Discipline } from '../models/Discipline';

export const compareSkiers = (s1: Skier, s2: Skier) =>
    s1.lastName.localeCompare(s2.lastName);

export const compareRace = (r1: Race, r2: Race) =>
    r1.raceDate.getTime() - r2.raceDate.getTime();

export const compareSeasons = (s1: Season, s2: Season) =>
    s1.startDate.getTime() - s2.startDate.getTime();

export const compareDiscipline = (d1: Discipline, d2: Discipline) =>
    d1.disciplineName.localeCompare(d2.disciplineName);
