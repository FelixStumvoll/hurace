import { Season } from '../models/Season';
import Axios from 'axios';
import { Race } from '../models/Race';
import { DisciplineData } from '../models/DisciplineData';
import { StartList } from '../models/StartList';
import { RaceRanking } from '../models/RaceRanking';
import { Skier } from '../models/Skier';
import { Discipline } from '../models/Discipline';
import { env } from '../environment/environment';
import { toIsoWithTimezone } from './timeConverter';
import { Country } from '../models/Country';
import { Gender } from '../models/Gender';
import { SkierCreateDto } from '../models/SkierCreateDto';
import { SkierUpdateDto } from '../models/SkierUpdateDto';
import { SeasonCreateDto } from '../models/SeasonCreateDto';
import { SeasonUpdateDto } from '../models/SeasonUpdateDto';

//#region Season
const setSeasonDate = (season: Season) => {
    season.endDate = new Date(season.endDate);
    season.startDate = new Date(season.startDate);
};

export const getSeasons = async (): Promise<Season[]> => {
    var response = await Axios.get<Season[]>(`${env.apiUrl}/season`);
    response.data.forEach(setSeasonDate);
    return response.data;
};

export const createSeason = async (
    season: SeasonCreateDto
): Promise<number> => {
    season.startDate = toIsoWithTimezone(season.startDate);
    season.endDate = toIsoWithTimezone(season.endDate);
    return (await Axios.put<Season>(`${env.apiUrl}/season`, season)).data.id;
};

export const updateSeason = async (season: SeasonUpdateDto): Promise<void> => {
    season.startDate = toIsoWithTimezone(season.startDate);
    season.endDate = toIsoWithTimezone(season.endDate);
    await Axios.put(`${env.apiUrl}/season/${season.id}`, season);
};

export const getSeasonById = async (seasonId: number): Promise<Season> => {
    var response = await Axios.get<Season>(`${env.apiUrl}/season/${seasonId}`);
    setSeasonDate(response.data);
    return response.data;
};
export const getRacesForSeason = async (
    seasonId: number
): Promise<DisciplineData[]> => {
    let response = await Axios.get<Race[]>(
        `${env.apiUrl}/season/${seasonId}/races`
    );
    let disciplineMap: DisciplineData[] = [];
    response.data.forEach(r => {
        r.raceDate = new Date(r.raceDate);
        let disciplineEntry = disciplineMap.find(
            d => d.discipline.id === r.disciplineId
        );

        if (disciplineEntry === undefined)
            disciplineMap.push({
                discipline: r.discipline,
                races: [r]
            });
        else disciplineEntry.races.push(r);
    });

    return disciplineMap;
};

//#endregion

//#region Race

export const getRaceDetails = async (raceId: number): Promise<Race> => {
    let response = await Axios.get<Race>(`${env.apiUrl}/race/${raceId}`);
    response.data.raceDate = new Date(response.data.raceDate);
    return response.data;
};

export const getStartListForRace = async (
    raceId: number
): Promise<StartList[]> => {
    let response = await Axios.get<StartList[]>(
        `${env.apiUrl}/race/${raceId}/startList`
    );
    response.data.forEach(
        sl => (sl.skier.dateOfBirth = new Date(sl.skier.dateOfBirth))
    );
    return response.data;
};

export const getRankingForRace = async (
    raceId: number
): Promise<RaceRanking[]> => {
    let response = await Axios.get<RaceRanking[]>(
        `${env.apiUrl}/race/${raceId}/ranking`
    );
    response.data.forEach(rr => {
        setSkierDate(rr.startList.skier);
        if (rr.disqualified) return;
        rr.time = new Date(rr.time!);
        rr.timeToLeader = new Date(rr.timeToLeader!);
    });

    return response.data;
};

export const getWinnersForRace = async (
    raceId: number
): Promise<RaceRanking[]> => {
    let response = await Axios.get<RaceRanking[]>(
        `${env.apiUrl}/race/${raceId}/winners`
    );
    response.data.forEach(rr => {
        setSkierDate(rr.startList.skier);
        rr.time = new Date(rr.time!);
        rr.timeToLeader = new Date(rr.timeToLeader!);
    });

    return response.data;
};

//#endregion

//#region Skier

const setSkierDate = (skier: Skier) =>
    (skier.dateOfBirth = new Date(skier.dateOfBirth));

export const getSkiers = async (): Promise<Skier[]> => {
    let response = await Axios.get<Skier[]>(`${env.apiUrl}/skier`);
    response.data.forEach(setSkierDate);
    return response.data;
};

export const getSkierById = async (skierId: number): Promise<Skier> => {
    let response = await Axios.get<Skier>(`${env.apiUrl}/skier/${skierId}`);
    setSkierDate(response.data);
    return response.data;
};

export const getDisciplinesForSkier = async (
    skierId: number
): Promise<Discipline[]> =>
    (
        await Axios.get<Discipline[]>(
            `${env.apiUrl}/skier/${skierId}/disciplines`
        )
    ).data;

export const updateSkier = async (skier: SkierUpdateDto) => {
    skier.dateOfBirth = toIsoWithTimezone(skier.dateOfBirth);
    await Axios.put<Skier | SkierCreateDto>(
        `${env.apiUrl}/skier/${skier.id}`,
        skier
    );
};

export const createSkier = async (skier: SkierCreateDto): Promise<number> => {
    skier.dateOfBirth = toIsoWithTimezone(skier.dateOfBirth);
    let response = await Axios.put<Skier>(`${env.apiUrl}/skier`, skier);
    return response.data.id;
};

export const updateSkierDisciplines = async (
    skierId: number,
    disciplines: number[]
): Promise<void> => {
    await Axios.put(`${env.apiUrl}/skier/${skierId}/disciplines`, disciplines);
};

//#endregion

export const getAllCountries = async (): Promise<Country[]> =>
    (await Axios.get<Country[]>(`${env.apiUrl}/country`)).data;

export const getAllDisciplines = async (): Promise<Discipline[]> =>
    (await Axios.get<Discipline[]>(`${env.apiUrl}/discipline`)).data;

export const getAllGenders = async (): Promise<Gender[]> =>
    (await Axios.get<Gender[]>(`${env.apiUrl}/gender`)).data;