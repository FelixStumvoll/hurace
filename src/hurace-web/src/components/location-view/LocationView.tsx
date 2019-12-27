import React from 'react';
import styled from 'styled-components';
import { dummyLocations } from './dummySeasons';
import { LocationViewItem } from './LocationViewItem';

const LocationItemPanel = styled.div`
    display: flex;
    flex-wrap: wrap;
`;

export const LocationView: React.FC = () => {
    let locations = dummyLocations;

    return (
        <LocationItemPanel>
            {locations.map(l => (
                <LocationViewItem key={l.id} location={l} />
            ))}
        </LocationItemPanel>
    );
};
