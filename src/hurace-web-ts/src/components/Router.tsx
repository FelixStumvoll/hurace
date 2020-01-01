import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { SeasonView } from './season-view/SeasonView';
import { RaceDetailView } from './race-view/RaceDetailView';

export const Router: React.FC = () => {
    return (
        <Switch>
            <Route exact path="/season">
                <SeasonView />
            </Route>
            <Route
                exact
                path="/season/:seasonId/race/:raceId"
                render={({ match }) => (
                    <RaceDetailView raceId={Number(match.params.raceId)} />
                )}
            />
        </Switch>
    );
};
