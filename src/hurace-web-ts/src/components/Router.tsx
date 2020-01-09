import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { SeasonListView } from './season-list-view/SeasonListView';
import { RaceDetailView } from './race-detail-view/RaceDetailView';
import { SkierDetailView } from './skier-detail-view/SkierDetailView';
import { SkierListView } from './skier-list-view/SkierListView';
import { SeasonDetailView } from './season-detail-view/SeasonDetailView';
import { SeasonUpdateView } from './season-update-view/SeasonUpdateView';
import { SkierUpdateView } from './skier-update-view/SkierUpdateView';
import { ActiceRaceList } from './active-race-list-view/ActiceRaceList';
import { ActiveRaceDetailView } from './active-race-detail-view/ActiveRaceDetailView';
import { AuthRoute } from './shared/AuthRoute';

export const Router: React.FC = () => {
    const redirectUri = '/';

    return (
        <Switch>
            <Route exact path="/season">
                <SeasonListView />
            </Route>
            <AuthRoute redirectUri={redirectUri} exact path="/season/new">
                <SeasonUpdateView />
            </AuthRoute>
            <AuthRoute
                exact
                redirectUri={redirectUri}
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

            <Route exact path="/activeRace">
                <ActiceRaceList />
            </Route>

            <Route
                exact
                path="/activeRace/:raceId"
                render={({ match }) => (
                    <ActiveRaceDetailView raceId={match.params.raceId} />
                )}
            />
            <Route exact path="/skier">
                <SkierListView />
            </Route>

            <AuthRoute redirectUri={redirectUri} exact path="/skier/new">
                <SkierUpdateView />
            </AuthRoute>

            <AuthRoute
                exact
                redirectUri={redirectUri}
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
