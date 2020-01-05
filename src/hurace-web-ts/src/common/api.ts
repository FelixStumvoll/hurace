import { Season } from '../interfaces/Season';
import Axios from 'axios';
import { Race } from '../interfaces/Race';
import { DisciplineData } from '../interfaces/DisciplineData';
import { StartList } from '../interfaces/StartList';
import { RaceRanking } from '../interfaces/RaceRanking';
import { Skier } from '../interfaces/Skier';
import { Discipline } from '../interfaces/Discipline';
import { env } from '../environment/environment';

const setSkierDate = (skier: Skier) =>
    (skier.dateOfBirth = new Date(skier.dateOfBirth));

const setSeasonDate = (season: Season) => {
    season.endDate = new Date(season.endDate);
    season.startDate = new Date(season.startDate);
};

export const getSeasons = async (): Promise<Season[]> => {
    var response = await Axios.get<Season[]>(`${env.apiUrl}/season`);
    response.data.forEach(setSeasonDate);
    return response.data;
};

export const updateSeason = async (season: Season): Promise<void> => {
    var response = await Axios.put<Season>(`${env.apiUrl}/season`, season);
};

export const getSeasonById = async (seasonId: number): Promise<Season> => {
    var response = await Axios.get<Season>(`${env.apiUrl}/season/${seasonId}`);
    setSeasonDate(response.data);
    return response.data;
};

export const getRaceDetails = async (raceId: number): Promise<Race> => {
    let response = await Axios.get<Race>(`${env.apiUrl}/race/${raceId}`);
    response.data.raceDate = new Date(response.data.raceDate);
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
