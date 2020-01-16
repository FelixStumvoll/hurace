import React, { useCallback } from 'react';
import { HeaderCard } from '../../shared/HeaderCard';
import styled from 'styled-components';
import { SkierListViewItem } from '../../skier-list-view/SkierListViewItem';
import { RowFlex, TextBold } from '../../../theme/CustomComponents';
import { useAsync } from 'react-async-hook';
import { getWinnersForRace, getRaceDetails } from '../../../common/api';
import { LoadingWrapper } from '../../shared/LoadingWrapper';
import { RaceRanking } from '../../../models/RaceRanking';

const WinnerWrapper = styled.div`
    margin-left: ${props => props.theme.gap};
    display: flex;
    flex-direction: column;
    :first-child {
        margin-left: 0;
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
        <HeaderCard headerText="Sieger">
            <LoadingWrapper loading={loading} error={error}>
                <RowFlex>
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
                </RowFlex>
            </LoadingWrapper>
        </HeaderCard>
    );
};
