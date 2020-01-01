import { Season } from './interfaces/Season';
import Axios from 'axios';
import { Race } from './interfaces/Race';
import { DisciplineData } from './interfaces/DisciplineData';
import { StartList } from './interfaces/StartList';
import { RaceRanking } from './interfaces/RaceRanking';
import { Skier } from './interfaces/Skier';

const API_URL = 'http://localhost:5000';

export const getSeasons = async (): Promise<Season[]> => {
    var response = await Axios.get<Season[]>(`${API_URL}/season`);

    response.data.forEach(season => {
        season.endDate = new Date(season.endDate);
        season.startDate = new Date(season.startDate);
    });

    return response.data;
};

export const getRaceDetails = async (raceId: number): Promise<Race> => {
    let response = await Axios.get<Race>(`${API_URL}/race/${raceId}`);
    response.data.raceDate = new Date(response.data.raceDate);
    return response.data;
};

export const getRacesForSeason = async (
    seasonId: number
): Promise<DisciplineData[]> => {
    let response = await Axios.get<Race[]>(
        `${API_URL}/season/${seasonId}/races`
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

const setSkierDate = (skier: Skier) => {
    skier.dateOfBirth = new Date(skier.dateOfBirth);
};

export const getStartListForRace = async (
    raceId: number
): Promise<StartList[]> => {
    let response = await Axios.get<StartList[]>(
        `${API_URL}/race/${raceId}/startList`
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
        `${API_URL}/race/${raceId}/ranking`
    );
    response.data.forEach(rr => {
        setSkierDate(rr.startList.skier);
        if (rr.disqualified) return;
        rr.time = new Date(rr.time!);
        rr.timeToLeader = new Date(rr.timeToLeader!);
    });

    return response.data;
};
