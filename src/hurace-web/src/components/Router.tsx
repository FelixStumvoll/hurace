import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { SeasonView } from './season-view/SeasonView';
import { LocationView } from './location-view/LocationView';

export const Router: React.FC = () => {
    return (
        <Switch>
            <Route exact path="/seasons">
                <SeasonView />
            </Route>
            <Route exact path="/seasons/:id/locations">
                <LocationView />
            </Route>
        </Switch>
    );
};
