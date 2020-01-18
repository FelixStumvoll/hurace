import React from 'react';
import styled from 'styled-components';

import { RaceDetailPanel } from '../shared/race/RaceDetailPanel';

import { CurrentSkierPanel } from './CurrentSkierPanel';
import { SplitTimeListView } from './split-time-view/SplitTimeListView';
import { RowFlex, ColumnFlex, AlignRight } from '../../theme/CustomComponents';
import { StartListView } from '../shared/race/startlist-view/StartListView';
import { RankingListView } from '../shared/race/ranking-view/RankingListView';
import { useLiveRace } from './useLiveRace';

const InfoFlex = styled(RowFlex)`
    @media screen and (max-width: ${props => props.theme.mobileSize}) {
        flex-direction: column;
    }
`;

const DetailPanelWrapper = styled.div`
    height: fit-content;
    margin-right: ${props => props.theme.gap};

    @media screen and (max-width: ${props => props.theme.mobileSize}) {
        margin-right: 0px;
    }
`;

const CurrentSkierWrapper = styled(AlignRight)`
    @media screen and (max-width: ${props => props.theme.mobileSize}) {
        margin-left: 0px;
        margin-top: ${props => props.theme.gap};
    }
`;

const ListGrid = styled.div`
    display: grid;
    gap: ${props => props.theme.gap};
    margin-top: ${props => props.theme.gap};
    grid-template-columns: 1fr 1fr auto;
    grid-template-areas: 'startlist ranking splittime';

    @media screen and (max-width: ${props => props.theme.mobileSize}) {
        grid-template-columns: 1fr;
        grid-template-areas: 'splittime' 'ranking' 'startlist';
    }
`;

const StartListWrapper = styled.div`
    grid-area: startlist;
`;

const RankingWrapper = styled.div`
    grid-area: ranking;
`;

const SplitTimeWrapper = styled.div`
    grid-area: splittime;
`;

export const ActiveRaceDetailView: React.FC<{ raceId: number }> = ({
    raceId
}) => {
    const [startList, ranking, currentSkier, splitTimes] = useLiveRace(raceId);

    return (
        <ColumnFlex>
            <InfoFlex>
                <DetailPanelWrapper>
                    <RaceDetailPanel raceId={raceId} />
                </DetailPanelWrapper>

                <CurrentSkierWrapper>
                    <CurrentSkierPanel currentSkier={currentSkier} />
                </CurrentSkierWrapper>
            </InfoFlex>
            <ListGrid>
                {startList && (
                    <StartListWrapper>
                        <StartListView startList={startList} />
                    </StartListWrapper>
                )}

                {ranking && (
                    <RankingWrapper>
                        <RankingListView raceRanking={ranking} />
                    </RankingWrapper>
                )}
                <SplitTimeWrapper>
                    <SplitTimeListView
                        currentSkier={currentSkier}
                        splitTimes={splitTimes}
                    />
                </SplitTimeWrapper>
            </ListGrid>
        </ColumnFlex>
    );
};
