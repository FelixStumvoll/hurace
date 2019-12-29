import { SeasonAction } from './seasonActionTypes';
import { Season } from '../../../interfaces/Season';

export interface SeasonReducerState {
    seasons: Season[];
}

const initialState: SeasonReducerState = {
    seasons: [
        { id: 1, startDate: new Date('1999'), endDate: new Date('2000') },
        { id: 2, startDate: new Date('2000'), endDate: new Date('2001') },
        { id: 3, startDate: new Date('2001'), endDate: new Date('2002') },
        { id: 4, startDate: new Date('2002'), endDate: new Date('2003') },
        { id: 5, startDate: new Date('2003'), endDate: new Date('2004') },
        { id: 6, startDate: new Date('2004'), endDate: new Date('2005') }
    ]
};

export const seasonReducer = (state = initialState, action: SeasonAction) => {
            return state;
};