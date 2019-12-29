import { combineReducers } from 'redux';
import { connectRouter, RouterState } from 'connected-react-router';
import { History } from 'history';
import {
    breadcrumbsReducer,
    BreadcrumbsReducerState
} from './reducers/breadcrumbsReducer/breadcrumbsReducer';
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
        breadcrumbs: breadcrumbsReducer,
        seasons: seasonReducer,
        locations: locationReducer,
        races: raceReducer,
        disciplines: disciplineReducer,
        seasonExpander: seasonExpanderReducer,
        router: connectRouter(history)
    });

export type StoreState = {
    breadcrumbs: BreadcrumbsReducerState;
    router: RouterState<any>;
    seasons: SeasonReducerState;
    locations: LocationReducerState;
    disciplines: DisciplineReducerState;
    seasonExpander: SeasonExpanderReducerState;
    races: RaceReducerState;
};
