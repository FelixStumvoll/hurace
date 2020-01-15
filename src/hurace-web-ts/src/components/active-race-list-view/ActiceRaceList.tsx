import React from 'react';
import { getActiveRaces } from '../../common/api';
import { RaceListViewItem } from '../shared/race/RaceListViewItem';
import { DefaultLink, FlexWrap } from '../../theme/CustomComponents';
import { LoadingWrapper } from '../shared/LoadingWrapper';
import { useAsync } from 'react-async-hook';

export const ActiceRaceList: React.FC = () => {
    const { loading, error, result: activeRaces } = useAsync(
        getActiveRaces,
        []
    );

    return (
        <LoadingWrapper loading={loading} error={error}>
            <FlexWrap>
                <FlexWrap>
                    {activeRaces?.map(ar => (
                        <DefaultLink key={ar.id} to={`/activeRace/${ar.id}`}>
                            <RaceListViewItem race={ar} />
                        </DefaultLink>
                    ))}
                </FlexWrap>
            </FlexWrap>
        </LoadingWrapper>
    );
};
