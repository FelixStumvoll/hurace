import React from 'react';
import styled from 'styled-components';
import { RaceDetailPanel } from './RaceDetailPanel';
import { RaceListsPanel } from './RaceListsPanel';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';

const RacePanel = styled.div`
    display: grid;
    max-height: 100%;
    grid-row-gap: 10px;
    grid-template-rows: auto 1fr;
    overflow: hidden;
`;

export const RaceDetailView: React.FC<{ raceId: number; seasonId: number }> = ({
    raceId,
    seasonId
}) => {
    return (
        <DetailViewWrapper
            url={`/season/${seasonId}`}
            backText=" Zurück zur Saison"
        >
            <RacePanel>
                <RaceDetailPanel raceId={raceId} />
                <RaceListsPanel raceId={raceId} />
            </RacePanel>
        </DetailViewWrapper>
    );
};
