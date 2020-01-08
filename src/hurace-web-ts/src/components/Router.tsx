import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { SeasonListView } from './season-list-view/SeasonView';
import { RaceDetailView } from './race-view/RaceDetailView';
import { SkierDetailView } from './skier-detail-view/SkierDetailView';
import { SkierListView } from './skier-list-view/SkierListView';
import { SeasonDetailView } from './season-detail-view/SeasonDetailView';
import { SeasonUpdateView } from './season-update-view/SeasonUpdateView';
import { SkierUpdateView } from './skier-update-view/SkierUpdateView';

export const Router: React.FC = () => {
    return (
        <Switch>
            <Route exact path="/season">
                <SeasonListView />
            </Route>
            <Route exact path="/season/new">
                <SeasonUpdateView />
            </Route>
            <Route
                exact
                path="/season/:seasonId/update"
                render={({ match }) => (
                    <SeasonUpdateView
                        seasonId={Number(match.params.seasonId)}
                    />
                )}
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
                <SkierListView />
            </Route>

            <Route exact path="/skier/new">
                <SkierUpdateView />
            </Route>

            <Route
                exact
                path="/skier/:skierId/update"
                render={({ match }) => (
                    <SkierUpdateView skierId={Number(match.params.skierId)} />
                )}
            />

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
