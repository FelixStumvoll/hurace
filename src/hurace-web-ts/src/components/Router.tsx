import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { SeasonView } from './season-view/SeasonView';
import { RaceDetailView } from './RaceDetailView';

export const Router: React.FC = () => {
    return (
        <Switch>
            <Route exact path="/seasons">
                <SeasonView />
            </Route>
            <Route
                exact
                path="/races/:raceId"
                render={({ match }) => (
                    <RaceDetailView raceId={Number(match.params.raceId)} />
                )}
            />
        </Switch>
    );
};
