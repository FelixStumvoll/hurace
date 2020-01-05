import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { SeasonView } from './season-view/SeasonView';
import { RaceDetailView } from './race-view/RaceDetailView';
import { SkierDetailView } from './skier-detail-view/SkierDetailView';
import { SkierView } from './skier-view/SkierView';
import { SeasonDetailView } from './season-detail-view/SeasonDetailView';
import { SeasonUpdateView } from './season-update-view/SeasonUpdateView';

export const Router: React.FC = () => {
    return (
        <Switch>
            <Route exact path="/season">
                <SeasonView />
            </Route>
            <Route
                exact
                path="/season/new"
                render={({ match }) => <SeasonUpdateView />}
            />
            <Route
                exact
                path="/season/:seasonId/"
                render={({ match }) => (
                    <SeasonDetailView
                        seasonId={Number(match.params.seasonId)}
                    />
                )}
            />

            <Route
                exact
                path="/season/:seasonId/race/:raceId"
                render={({ match }) => (
                    <RaceDetailView
                        seasonId={Number(match.params.seasonId)}
                        raceId={Number(match.params.raceId)}
                    />
                )}
            />
            <Route exact path="/skier">
                <SkierView />
            </Route>

            <Route
                exact
                path="/skier/:skierId"
                render={({ match }) => (
                    <SkierDetailView skierId={Number(match.params.skierId)} />
                )}
            />
        </Switch>
    );
};
