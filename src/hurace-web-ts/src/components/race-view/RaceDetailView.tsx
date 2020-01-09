import React from 'react';
import styled from 'styled-components';
import { RaceDetailPanel } from './RaceDetailPanel';
import { RaceListsPanel } from './RaceListsPanel';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { useStateAsync } from '../../hooks/useStateAsync';
import { getRaceDetails, getWinnersForRace } from '../../common/api';
import { RaceWinnersView } from './winners-view/RaceWinnersView';

const RacePanel = styled.div`
    display: flex;
    flex-direction: column;
`;

const RaceDetails = styled.div`
    display: flex;
    margin-bottom: ${props => props.theme.gap};
`;

const DetailPanelWrapper = styled.div`
    height: fit-content;
    margin-right: ${props => props.theme.gap};
`;

const WinnersWrapper = styled.div`
    margin-left: auto;
`;

export const RaceDetailView: React.FC<{ raceId: number; seasonId: number }> = ({
    raceId,
    seasonId
}) => {
    const [race] = useStateAsync(getRaceDetails, raceId);
    const [winners] = useStateAsync(getWinnersForRace, raceId);

    return (
        <DetailViewWrapper
            url={`/season/${seasonId}`}
            backText=" Zurück zur Saison"
        >
            <RacePanel>
                <RaceDetails>
                    <DetailPanelWrapper>
                        {race && <RaceDetailPanel race={race} />}
                    </DetailPanelWrapper>

                    {winners && winners.length > 0 && (
                        <WinnersWrapper>
                            <RaceWinnersView winners={winners} />
                        </WinnersWrapper>
                    )}
                </RaceDetails>

                <RaceListsPanel raceId={raceId} />
            </RacePanel>
        </DetailViewWrapper>
    );
};
