import React from 'react';
import styled from 'styled-components';
import { RaceDetailPanel } from './RaceDetailPanel';
import { RaceListsPanel } from './RaceListsPanel';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { useStateAsync } from '../../hooks/useStateAsync';
import { getRaceDetails } from '../../common/api';

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
    const [race] = useStateAsync(getRaceDetails, raceId);

    return (
        <DetailViewWrapper
            url={`/season/${seasonId}`}
            backText=" Zurück zur Saison"
        >
            <RacePanel>
                {race && <RaceDetailPanel race={race} />}
                <RaceListsPanel raceId={raceId} />
            </RacePanel>
        </DetailViewWrapper>
    );
};
