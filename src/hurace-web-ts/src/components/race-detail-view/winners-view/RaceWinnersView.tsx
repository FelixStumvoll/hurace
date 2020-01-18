import React, { useCallback } from 'react';
import { HeaderCard } from '../../shared/HeaderCard';
import styled from 'styled-components';
import { SkierListViewItem } from '../../skier-list-view/SkierListViewItem';
import { RowFlex, TextBold } from '../../../theme/CustomComponents';
import { useAsync } from 'react-async-hook';
import { getWinnersForRace, getRaceDetails } from '../../../common/api';
import { LoadingWrapper } from '../../shared/LoadingWrapper';
import { RaceRanking } from '../../../models/RaceRanking';

const WinnerFlex = styled(RowFlex)`
    flex-wrap: wrap;
    justify-content: center;
    @media screen and (max-width: ${props => props.theme.mobileSize}) {
        flex-direction: column;
        align-items: center;
    }
`;

const WinnerWrapper = styled.div`
    margin-right: ${props => props.theme.gap};
    margin-bottom: ${props => props.theme.gap};
    display: flex;
    flex-direction: column;
    :last-child {
        margin-right: 0;
    }

    @media screen and (max-width: ${props => props.theme.mobileSize}) {
        margin-left: 0;
        margin-top: ${props => props.theme.gap};
    }
`;

const WinnerPosition = styled(TextBold)`
    margin-bottom: 10px;
`;

export const RaceWinnersView: React.FC<{ raceId: number }> = ({ raceId }) => {
    const { loading, error, result: winnerData } = useAsync(async (): Promise<
        [number, RaceRanking[] | undefined]
    > => {
        let race = await getRaceDetails(raceId);
        if (race.raceStateId !== 3) return [race.raceStateId, undefined];
        else return [race.raceStateId, await getWinnersForRace(raceId)];
    }, [raceId]);

    const getErrorText = useCallback((state: number): string => {
        switch (state) {
            case 1:
                return 'Rennen hat noch nicht begonnen';
            case 2:
                return 'Rennen läuft gerade';
            case 4:
                return 'Rennen wurde abgebrochen';
        }
        return '';
    }, []);

    return (
        <HeaderCard
            contentStyles={{ paddingBottom: '0px' }}
            headerText="Sieger"
        >
            <LoadingWrapper loading={loading} error={error}>
                <WinnerFlex>
                    {winnerData && winnerData[0] === 3 ? (
                        winnerData[1]?.map(w => (
                            <WinnerWrapper key={w.position}>
                                <WinnerPosition>{w.position}</WinnerPosition>
                                <SkierListViewItem skier={w.skier} />
                            </WinnerWrapper>
                        ))
                    ) : (
                        <div>{winnerData && getErrorText(winnerData?.[0])}</div>
                    )}
                </WinnerFlex>
            </LoadingWrapper>
        </HeaderCard>
    );
};
