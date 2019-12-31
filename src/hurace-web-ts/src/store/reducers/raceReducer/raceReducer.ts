import { Race } from '../../../interfaces/Race';
import { RaceAction } from './raceActionTypes';

export interface RaceReducerState {
    races: Race[];
}

const initialState: RaceReducerState = {
    races: [
        // {
        //     id: 1,
        //     disciplineId: 1,
        //     genderId: 1,
        //     location: { id: 1, locationName: 'Despacito' },
        //     discipline: { disciplineName: 'downhill', id: 1 },
        //     locationId: 1,
        //     raceDate: new Date(),
        //     raceStateId: 1,
        //     season: { startDate: new Date(), endDate: new Date(), id: 1 },
        //     seasonId: 1,
        //     raceDescription: 'Lorem Ipsum eeeeeeeeesdfdfgl-sdöfmf kgäösdm,äöewrr,löä#fw,e#äf,.we#ärf.,wer#äf,mwerpo#omfwerä#ölfmwer#pmpüw#eq#m+üw#eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeLorem Ipsum eeeeeeeeesdfdfgl-sdöfmf kgäösdm,äöewrr,löä#fw,e#äf,.we#ärf.,wer#äf,mwerpo#omfwerä#ölfmwer#pmpüw#eq#m+üw#eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee'
        // },
        // {
        //     id: 2,
        //     disciplineId: 1,
        //     genderId: 1,
        //     location: { id: 1, locationName: 'Despacito' },
        //     discipline: { disciplineName: 'downhill', id: 1 },
        //     locationId: 1,
        //     raceDate: new Date(),
        //     raceStateId: 1,
        //     season: { startDate: new Date(), endDate: new Date(), id: 1 },
        //     seasonId: 1
        // },
        // {
        //     id: 3,
        //     disciplineId: 1,
        //     genderId: 1,
        //     location: { id: 1, locationName: 'Despacito' },
        //     discipline: { disciplineName: 'downhill', id: 1 },
        //     locationId: 1,
        //     raceDate: new Date(),
        //     raceStateId: 1,
        //     season: { startDate: new Date(), endDate: new Date(), id: 1 },
        //     seasonId: 1
        // },
        // {
        //     id: 4,
        //     disciplineId: 1,
        //     genderId: 1,
        //     location: { id: 1, locationName: 'Despacito' },
        //     discipline: { disciplineName: 'downhill', id: 1 },
        //     locationId: 1,
        //     raceDate: new Date(),
        //     raceStateId: 1,
        //     season: { startDate: new Date(), endDate: new Date(), id: 1 },
        //     seasonId: 1
        // }
    ]
};

export const raceReducer = (state = initialState, action: RaceAction) => {
    return state;
};
