import React from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleLeft } from '@fortawesome/free-solid-svg-icons';
import { RaceDetailPanel } from './RaceDetailPanel';
import { RaceListsPanel } from './RaceListsPanel';

const RacePanel = styled.div`
    display: grid;
    height: 100%;
    grid-row-gap: 10px;
    grid-template-rows: auto auto 1fr;
`;

const BackLink = styled(Link)`
    text-decoration: none;
    color: black;
    font-size: 20px;
`;

const BackIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

export const RaceDetailView: React.FC<{ raceId: number }> = ({ raceId }) => {
    return (
        <RacePanel>
            <BackLink to="/season">
                <BackIcon icon={faAngleLeft} />
                <span>Zur Saisonübersicht</span>
            </BackLink>
            <RaceDetailPanel raceId={raceId} />
            <RaceListsPanel raceId={raceId} />
        </RacePanel>
    );
};
