import React from 'react';
import styled from 'styled-components';
import { RaceDetailPanel } from './RaceDetailPanel';
import { RaceListsPanel } from './RaceListsPanel';
import { DetailViewWrapper } from '../../shared/DetailViewWrapper';
import { useStateAsync } from '../../../hooks/useStateAsync';
import { getRaceDetails, getWinnersForRace } from '../../../common/api';
import { RaceWinnersView } from './winners-view/RaceWinnersView';
import {
    ColumnFlex,
    AlignRight,
    RowFlex
} from '../../../theme/CustomComponents';

const RaceDetails = styled(RowFlex)`
    margin-bottom: ${props => props.theme.gap};
`;

const DetailPanelWrapper = styled.div`
    height: fit-content;
    margin-right: ${props => props.theme.gap};
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
            <ColumnFlex>
                <RaceDetails>
                    <DetailPanelWrapper>
                        {race && <RaceDetailPanel race={race} />}
                    </DetailPanelWrapper>

                    {winners && winners.length > 0 && (
                        <AlignRight>
                            <RaceWinnersView winners={winners} />
                        </AlignRight>
                    )}
                </RaceDetails>

                <RaceListsPanel raceId={raceId} />
            </ColumnFlex>
        </DetailViewWrapper>
    );
};
