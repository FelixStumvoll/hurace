import React from 'react';
import { Race } from '../../interfaces/Race';
import styled from 'styled-components';
import { Card } from '../../theme/StyledComponents';
import { Link } from 'react-router-dom';

const RaceLink = styled(Link)`
    text-decoration: none;
    color: black;
`;

const RaceCard = styled(Card)`
    padding: 15px;
    margin: 0px 10px 10px 0px;
    display: grid;
    grid-column-gap: 10px;
    grid-template-areas: 'location date' 'gender state';
`;

const RaceLocation = styled.div`
    grid-area: location;
`;

const RaceDate = styled.div`
    grid-area: date;
`;

const RaceGender = styled.div`
    grid-area: gender;
`;

const RaceState = styled.div`
    grid-area: state;
`;

export const RaceViewItem: React.FC<{ race: Race }> = ({ race }) => {
    return (
        <RaceLink to={`/races/${race.id}`}>
            <RaceCard>
                <RaceLocation>{race.location.locationName}</RaceLocation>
                <RaceDate>{race.raceDate.toDateString()}</RaceDate>
                <RaceGender>Herren</RaceGender>
            </RaceCard>
        </RaceLink>
    );
};
