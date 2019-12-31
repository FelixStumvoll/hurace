import { combineReducers } from 'redux';
import { connectRouter, RouterState } from 'connected-react-router';
import { History } from 'history';
import {
    seasonReducer,
    SeasonReducerState
} from './reducers/seasonReducer/seasonReducer';
import {
    locationReducer,
    LocationReducerState
} from './reducers/locationReducer/locationReducer';
import {
    disciplineReducer,
    DisciplineReducerState
} from './reducers/disciplineReducer/disciplineReducer';
import {
    raceReducer,
    RaceReducerState
} from './reducers/raceReducer/raceReducer';
import {
    seasonExpanderReducer,
    SeasonExpanderReducerState
} from './reducers/seasonExpanderReducer/seasonExpanderReducer';

export const rootReducer = (history: History<any>) =>
    combineReducers({
        seasons: seasonReducer,
        locations: locationReducer,
        races: raceReducer,
        disciplines: disciplineReducer,
        seasonExpander: seasonExpanderReducer,
        router: connectRouter(history)
    });

export type StoreState = {
    router: RouterState<any>;
    seasons: SeasonReducerState;
    locations: LocationReducerState;
    disciplines: DisciplineReducerState;
    seasonExpander: SeasonExpanderReducerState;
    races: RaceReducerState;
};
