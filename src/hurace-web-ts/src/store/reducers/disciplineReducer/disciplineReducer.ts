import { Discipline } from '../../../interfaces/Discipline';
import { DisciplineAction } from './disciplineActionTypes';

export interface DisciplineReducerState {
    disciplines: Discipline[];
}

const initialState: DisciplineReducerState = {
    disciplines: [
        { id: 1, disciplineName: 'Downhill' },
        { id: 2, disciplineName: 'Super-G' }
    ]
};

export const disciplineReducer = (
    state = initialState,
    action: DisciplineAction
) => {
    return state;
};
