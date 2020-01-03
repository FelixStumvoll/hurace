import React from 'react';
import styled from 'styled-components';
import { RaceDetailPanel } from './RaceDetailPanel';
import { RaceListsPanel } from './RaceListsPanel';
import { BackLinkWrapper } from '../BackLinkWrapper';

const RacePanel = styled.div`
    display: grid;
    max-height: 100%;
    grid-row-gap: 10px;
    grid-template-rows: auto 1fr;
    overflow: hidden;
`;

export const RaceDetailView: React.FC<{ raceId: number }> = ({ raceId }) => {
    return (
        <BackLinkWrapper url="/season" backText=" Zurück zur Saisonübersicht">
            <RacePanel>
                <RaceDetailPanel raceId={raceId} />
                <RaceListsPanel raceId={raceId} />
            </RacePanel>
        </BackLinkWrapper>
    );
};
