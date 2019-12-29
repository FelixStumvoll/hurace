import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { SeasonView } from './season-view/SeasonView';
import { LocationView } from './location-view/LocationView';
import { useSelector, useDispatch } from 'react-redux';
import { StoreState } from '../store/rootReducer';
import { clearBreadcrumbs } from '../store/reducers/breadcrumbsReducer/breadcrumbsActions';
import { RaceDetailView } from './RaceDetailView';

export const Router: React.FC = () => {
    var route = useSelector((state: StoreState) => state.router);
    var dispatch = useDispatch();

    dispatch(clearBreadcrumbs());
    // route.
    return (
        <Switch>
            <Route exact path="/seasons">
                <SeasonView />
            </Route>
            {/* <Route
                exact
                path="/seasons/:seasonId/"
                render={({ match }) => (
                    <LocationView seasonId={match.params.seasonId} />
                )}
            /> */}
            <Route
                exact
                path="/races/:raceId"
                render={({ match }) => (
                    <RaceDetailView raceId={match.params.raceId} />
                )}
            />
        </Switch>
    );
};
